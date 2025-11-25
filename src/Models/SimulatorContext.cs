using System.Text.Json;

namespace RaythaSimulator.Models;

/// <summary>
/// Context object that will be passed to the Fluid template engine.
/// Wraps the sample data and provides access to all template variables.
/// </summary>
public class SimulatorContext
{
    public object? Target { get; set; }
    public ContentTypeModel? ContentType { get; set; }
    public OrganizationModel? CurrentOrganization { get; set; }
    public UserModel? CurrentUser { get; set; }
    public string PathBase { get; set; } = string.Empty;
    public Dictionary<string, string> QueryParams { get; set; } = new();

    /// <summary>
    /// Creates a SimulatorContext from the parsed sample data
    /// </summary>
    public static SimulatorContext FromSampleData(SampleData sampleData)
    {
        var context = new SimulatorContext
        {
            ContentType = sampleData.ContentType ?? new ContentTypeModel(),
            CurrentOrganization = sampleData.CurrentOrganization ?? new OrganizationModel { OrganizationName = "Sample Organization" },
            CurrentUser = sampleData.CurrentUser ?? new UserModel(),
            PathBase = sampleData.PathBase,
            QueryParams = sampleData.QueryParams ?? new Dictionary<string, string>()
        };

        // Convert Target from JsonElement to a dynamic object
        context.Target = ConvertJsonElement(sampleData.Target);

        return context;
    }

    /// <summary>
    /// Converts a JsonElement to a CLR object that Fluid can work with
    /// </summary>
    public static object? ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => ConvertJsonObject(element),
            JsonValueKind.Array => ConvertJsonArray(element),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Undefined => null,
            _ => null
        };
    }

    private static Dictionary<string, object?> ConvertJsonObject(JsonElement element)
    {
        var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in element.EnumerateObject())
        {
            dict[property.Name] = ConvertJsonElement(property.Value);
        }
        return dict;
    }

    private static List<object?> ConvertJsonArray(JsonElement element)
    {
        var list = new List<object?>();
        foreach (var item in element.EnumerateArray())
        {
            list.Add(ConvertJsonElement(item));
        }
        return list;
    }
}

