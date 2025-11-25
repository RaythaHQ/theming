using System.Text.Json;
using System.Text.RegularExpressions;
using RaythaSimulator;
using RaythaSimulator.Models;

// Parse command line arguments
string? sampleDataFile = null;
string? outputPath = null;
bool renderAll = false;

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--output" || args[i] == "-o")
    {
        if (i + 1 < args.Length)
        {
            outputPath = args[++i];
        }
        else
        {
            Console.Error.WriteLine("Error: --output requires a path argument");
            return 1;
        }
    }
    else if (args[i] == "--help" || args[i] == "-h")
    {
        PrintUsage();
        return 0;
    }
    else if (args[i] == "--all" || args[i] == "-a")
    {
        renderAll = true;
    }
    else if (!args[i].StartsWith('-'))
    {
        sampleDataFile = args[i];
    }
}

// Determine directories
var workingDir = Directory.GetCurrentDirectory();
var projectRoot = FindProjectRoot(workingDir);
var liquidDir = Path.Combine(projectRoot, "liquid");
var defaultSampleDataDir = Path.Combine(projectRoot, "src", "sample-data");
var htmlDir = outputPath ?? Path.Combine(projectRoot, "html");

if (!Directory.Exists(liquidDir))
{
    Console.Error.WriteLine($"Error: Liquid templates directory not found: {liquidDir}");
    return 1;
}

// Ensure output directory exists
Directory.CreateDirectory(htmlDir);

// If no file specified or --all, render all sample data files
if (renderAll || string.IsNullOrEmpty(sampleDataFile))
{
    return RenderAllSampleData(defaultSampleDataDir, liquidDir, htmlDir);
}
else
{
    var sampleDataPath = Path.GetFullPath(sampleDataFile, workingDir);
    if (!File.Exists(sampleDataPath))
    {
        Console.Error.WriteLine($"Error: Sample data file not found: {sampleDataPath}");
        return 1;
    }
    var sampleDataDir = Path.GetDirectoryName(sampleDataPath) ?? projectRoot;
    return RenderSampleDataFile(sampleDataPath, liquidDir, sampleDataDir, htmlDir);
}

static int RenderAllSampleData(string sampleDataDir, string liquidDir, string htmlDir)
{
    if (!Directory.Exists(sampleDataDir))
    {
        Console.Error.WriteLine($"Error: Sample data directory not found: {sampleDataDir}");
        return 1;
    }

    var jsonFiles = Directory.GetFiles(sampleDataDir, "*.json")
        .Where(f => !Path.GetFileName(f).Equals("menus.json", StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (jsonFiles.Count == 0)
    {
        Console.Error.WriteLine("Error: No sample data files found (excluding menus.json)");
        return 1;
    }

    Console.WriteLine($"Found {jsonFiles.Count} sample data file(s) to render");
    Console.WriteLine();

    int successCount = 0;
    int failCount = 0;

    foreach (var jsonFile in jsonFiles)
    {
        var result = RenderSampleDataFile(jsonFile, liquidDir, sampleDataDir, htmlDir);
        if (result == 0)
            successCount++;
        else
            failCount++;
        Console.WriteLine();
    }

    Console.WriteLine($"Rendering complete: {successCount} succeeded, {failCount} failed");
    return failCount > 0 ? 1 : 0;
}

static int RenderSampleDataFile(string sampleDataPath, string liquidDir, string sampleDataDir, string htmlDir)
{
    try
    {
        Console.WriteLine($"Loading sample data from: {sampleDataPath}");
        var json = File.ReadAllText(sampleDataPath);
        var sampleData = JsonSerializer.Deserialize<SampleData>(json);

        if (sampleData == null)
        {
            Console.Error.WriteLine("Error: Failed to parse sample data JSON");
            return 1;
        }

        if (string.IsNullOrEmpty(sampleData.LiquidFile))
        {
            Console.Error.WriteLine("Error: Sample data must specify 'liquid_file' property");
            return 1;
        }

        var timeZone = sampleData.CurrentOrganization?.TimeZone ?? "UTC";
        var renderEngine = new RenderEngine(liquidDir, sampleDataDir, timeZone);
        var inputFileName = Path.GetFileNameWithoutExtension(sampleDataPath);

        // Render the main template (list view or single item view)
        RenderMainTemplate(sampleData, renderEngine, liquidDir, htmlDir, inputFileName);

        // Render individual detail pages for items that have detail_liquid_file
        RenderDetailPages(sampleData, renderEngine, liquidDir, htmlDir);

        return 0;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.Error.WriteLine($"  Inner: {ex.InnerException.Message}");
        }
        return 1;
    }
}

static void RenderMainTemplate(SampleData sampleData, RenderEngine renderEngine, string liquidDir, string htmlDir, string inputFileName)
{
    var liquidFile = sampleData.LiquidFile;
    if (!liquidFile.EndsWith(".liquid", StringComparison.OrdinalIgnoreCase))
    {
        liquidFile += ".liquid";
    }

    var templatePath = Path.Combine(liquidDir, liquidFile);
    if (!File.Exists(templatePath))
    {
        throw new FileNotFoundException($"Template file not found: {templatePath}");
    }

    Console.WriteLine($"Loading template: {templatePath}");
    var templateSource = File.ReadAllText(templatePath);

    var context = SimulatorContext.FromSampleData(sampleData);

    Console.WriteLine("Rendering list/main template...");
    var html = renderEngine.RenderAsHtml(templateSource, context);

    var outputFileName = $"{inputFileName}.html";
    var outputFilePath = Path.Combine(htmlDir, outputFileName);

    File.WriteAllText(outputFilePath, html);
    Console.WriteLine($"Output written to: {outputFilePath}");
}

static void RenderDetailPages(SampleData sampleData, RenderEngine renderEngine, string liquidDir, string htmlDir)
{
    // Get items from Target
    var target = SimulatorContext.ConvertJsonElement(sampleData.Target) as Dictionary<string, object?>;
    if (target == null || !target.TryGetValue("Items", out var itemsObj) || itemsObj is not List<object?> items)
    {
        return;
    }

    // Filter items that have detail_liquid_file
    var itemsWithDetailTemplate = items
        .OfType<Dictionary<string, object?>>()
        .Where(item => item.TryGetValue("detail_liquid_file", out var val) && val is string s && !string.IsNullOrEmpty(s))
        .ToList();

    if (itemsWithDetailTemplate.Count == 0)
    {
        return;
    }

    Console.WriteLine($"Rendering {itemsWithDetailTemplate.Count} detail page(s)...");

    // Cache loaded templates
    var templateCache = new Dictionary<string, string>();

    foreach (var item in itemsWithDetailTemplate)
    {
        // Get the detail template for this item
        var detailLiquidFile = (string)item["detail_liquid_file"]!;
        if (!detailLiquidFile.EndsWith(".liquid", StringComparison.OrdinalIgnoreCase))
        {
            detailLiquidFile += ".liquid";
        }

        // Load template (with caching)
        if (!templateCache.TryGetValue(detailLiquidFile, out var detailTemplateSource))
        {
            var detailTemplatePath = Path.Combine(liquidDir, detailLiquidFile);
            if (!File.Exists(detailTemplatePath))
            {
                Console.Error.WriteLine($"  Warning: Detail template not found: {detailTemplatePath}");
                continue;
            }
            detailTemplateSource = File.ReadAllText(detailTemplatePath);
            templateCache[detailLiquidFile] = detailTemplateSource;
        }

        // Get the RoutePath to determine the output filename
        if (!item.TryGetValue("RoutePath", out var routePathObj) || routePathObj is not string routePath)
            continue;

        // The RoutePath should be the HTML filename (e.g., "posts_getting-started.html")
        var outputFileName = routePath;
        if (!outputFileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        {
            outputFileName += ".html";
        }

        // Create a detail context with this item as Target
        var detailContext = new SimulatorContext
        {
            Target = item,
            ContentType = sampleData.ContentType,
            CurrentOrganization = sampleData.CurrentOrganization ?? new OrganizationModel { OrganizationName = "Sample Organization" },
            CurrentUser = sampleData.CurrentUser ?? new UserModel(),
            PathBase = sampleData.PathBase,
            QueryParams = sampleData.QueryParams ?? new Dictionary<string, string>()
        };

        var html = renderEngine.RenderAsHtml(detailTemplateSource, detailContext);

        var outputFilePath = Path.Combine(htmlDir, outputFileName);
        File.WriteAllText(outputFilePath, html);
        Console.WriteLine($"  - {outputFileName}");
    }
}

static void PrintUsage()
{
    Console.WriteLine("Raytha Template Simulator");
    Console.WriteLine();
    Console.WriteLine("Usage: dotnet run [options] [sample-data.json]");
    Console.WriteLine();
    Console.WriteLine("Arguments:");
    Console.WriteLine("  <sample-data.json>    Path to a specific sample data JSON file (optional)");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  -a, --all             Render all sample data files (default if no file specified)");
    Console.WriteLine("  -o, --output <path>   Output directory for rendered HTML (default: html/)");
    Console.WriteLine("  -h, --help            Show this help message");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  dotnet run                              # Render all sample data files");
    Console.WriteLine("  dotnet run -- --all                     # Render all sample data files");
    Console.WriteLine("  dotnet run -- sample-data/posts.json    # Render single file");
    Console.WriteLine("  dotnet run -- --output ./output         # Render all to custom directory");
    Console.WriteLine();
    Console.WriteLine("Sample Data Format:");
    Console.WriteLine("  For list views with individual detail pages, include 'detail_liquid_file'");
    Console.WriteLine("  and set unique 'RoutePath' for each item in Target.Items.");
}

static string FindProjectRoot(string startDir)
{
    var dir = startDir;
    while (dir != null)
    {
        if (Directory.Exists(Path.Combine(dir, "liquid")) ||
            Directory.Exists(Path.Combine(dir, ".git")))
        {
            return dir;
        }
        dir = Directory.GetParent(dir)?.FullName;
    }
    return startDir;
}
