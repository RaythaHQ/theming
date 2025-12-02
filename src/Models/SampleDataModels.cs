using System.Text.Json;
using System.Text.Json.Serialization;

namespace RaythaSimulator.Models;

/// <summary>
/// Root structure for sample data JSON files
/// </summary>
public class SampleData
{
    [JsonPropertyName("liquid_file")]
    public string LiquidFile { get; set; } = string.Empty;

    [JsonPropertyName("Target")]
    public JsonElement Target { get; set; }

    [JsonPropertyName("ContentType")]
    public ContentTypeModel? ContentType { get; set; }

    [JsonPropertyName("CurrentOrganization")]
    public OrganizationModel? CurrentOrganization { get; set; }

    [JsonPropertyName("CurrentUser")]
    public UserModel? CurrentUser { get; set; }

    [JsonPropertyName("PathBase")]
    public string PathBase { get; set; } = string.Empty;

    [JsonPropertyName("QueryParams")]
    public Dictionary<string, string>? QueryParams { get; set; }
}

/// <summary>
/// Content type metadata
/// </summary>
public class ContentTypeModel
{
    [JsonPropertyName("LabelPlural")]
    public string LabelPlural { get; set; } = string.Empty;

    [JsonPropertyName("LabelSingular")]
    public string LabelSingular { get; set; } = string.Empty;

    [JsonPropertyName("DeveloperName")]
    public string DeveloperName { get; set; } = string.Empty;

    [JsonPropertyName("Description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("ContentTypeFields")]
    public List<ContentTypeFieldModel>? ContentTypeFields { get; set; }
}

/// <summary>
/// Content type field definition
/// </summary>
public class ContentTypeFieldModel
{
    [JsonPropertyName("Label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("DeveloperName")]
    public string DeveloperName { get; set; } = string.Empty;

    [JsonPropertyName("FieldType")]
    public string FieldType { get; set; } = string.Empty;

    [JsonPropertyName("Choices")]
    public List<ChoiceModel>? Choices { get; set; }
}

/// <summary>
/// Choice for dropdown/radio/multiple select fields
/// </summary>
public class ChoiceModel
{
    [JsonPropertyName("Label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("DeveloperName")]
    public string DeveloperName { get; set; } = string.Empty;
}

/// <summary>
/// Organization settings
/// </summary>
public class OrganizationModel
{
    [JsonPropertyName("OrganizationName")]
    public string OrganizationName { get; set; } = string.Empty;

    [JsonPropertyName("TimeZone")]
    public string TimeZone { get; set; } = "UTC";
}

/// <summary>
/// Current user context
/// </summary>
public class UserModel
{
    [JsonPropertyName("IsAuthenticated")]
    public bool IsAuthenticated { get; set; }

    [JsonPropertyName("IsAdmin")]
    public bool IsAdmin { get; set; }

    [JsonPropertyName("UserId")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("FirstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("LastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("FullName")]
    public string FullName => $"{FirstName} {LastName}".Trim();

    [JsonPropertyName("EmailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("Roles")]
    public List<string>? Roles { get; set; }

    [JsonPropertyName("UserGroups")]
    public List<string>? UserGroups { get; set; }

    [JsonPropertyName("LastModificationTime")]
    public DateTime? LastModificationTime { get; set; }
}

/// <summary>
/// Navigation menu model
/// </summary>
public class NavigationMenuModel
{
    [JsonPropertyName("Id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("Label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("DeveloperName")]
    public string DeveloperName { get; set; } = string.Empty;

    [JsonPropertyName("IsMainMenu")]
    public bool IsMainMenu { get; set; }

    [JsonPropertyName("MenuItems")]
    public List<NavigationMenuItemModel>? MenuItems { get; set; }
}

/// <summary>
/// Navigation menu item model
/// </summary>
public class NavigationMenuItemModel
{
    [JsonPropertyName("Id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("Label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("Url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("IsDisabled")]
    public bool IsDisabled { get; set; }

    [JsonPropertyName("OpenInNewTab")]
    public bool OpenInNewTab { get; set; }

    [JsonPropertyName("CssClassName")]
    public string? CssClassName { get; set; }

    [JsonPropertyName("Ordinal")]
    public int Ordinal { get; set; }

    [JsonPropertyName("IsFirstItem")]
    public bool IsFirstItem { get; set; }

    [JsonPropertyName("IsLastItem")]
    public bool IsLastItem { get; set; }

    [JsonPropertyName("MenuItems")]
    public List<NavigationMenuItemModel>? MenuItems { get; set; }
}

/// <summary>
/// Menus data file structure
/// </summary>
public class MenusData
{
    [JsonPropertyName("menus")]
    public List<NavigationMenuModel>? Menus { get; set; }
}

#region Site Pages

/// <summary>
/// Site pages data file structure
/// </summary>
public class SitePagesData
{
    [JsonPropertyName("CurrentOrganization")]
    public OrganizationModel? CurrentOrganization { get; set; }

    [JsonPropertyName("CurrentUser")]
    public UserModel? CurrentUser { get; set; }

    [JsonPropertyName("PathBase")]
    public string PathBase { get; set; } = string.Empty;

    [JsonPropertyName("pages")]
    public List<SitePageModel>? Pages { get; set; }
}

/// <summary>
/// Site page model
/// </summary>
public class SitePageModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("routePath")]
    public string RoutePath { get; set; } = string.Empty;

    [JsonPropertyName("isPublished")]
    public bool IsPublished { get; set; } = true;

    [JsonPropertyName("webTemplateDeveloperName")]
    public string WebTemplateDeveloperName { get; set; } = string.Empty;

    /// <summary>
    /// Widgets organized by section name. Key is section name, value is list of widgets.
    /// </summary>
    [JsonPropertyName("publishedWidgets")]
    public Dictionary<string, List<SitePageWidgetModel>>? PublishedWidgets { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Widget instance within a site page section
/// </summary>
public class SitePageWidgetModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("widgetType")]
    public string WidgetType { get; set; } = string.Empty;

    [JsonPropertyName("settingsJson")]
    public string SettingsJson { get; set; } = "{}";

    [JsonPropertyName("row")]
    public int Row { get; set; } = 1;

    [JsonPropertyName("column")]
    public int Column { get; set; } = 1;

    [JsonPropertyName("columnSpan")]
    public int ColumnSpan { get; set; } = 12;

    [JsonPropertyName("cssClass")]
    public string? CssClass { get; set; }

    [JsonPropertyName("htmlId")]
    public string? HtmlId { get; set; }

    [JsonPropertyName("customAttributes")]
    public string? CustomAttributes { get; set; }
}

/// <summary>
/// Widget data used during rendering (with parsed settings)
/// </summary>
public class SitePageWidgetRenderData
{
    public string Id { get; set; } = string.Empty;
    public string WidgetType { get; set; } = string.Empty;
    public string SettingsJson { get; set; } = "{}";
    public int Row { get; set; }
    public int Column { get; set; }
    public int ColumnSpan { get; set; }
    public string? CssClass { get; set; }
    public string? HtmlId { get; set; }
    public string? CustomAttributes { get; set; }

    public static SitePageWidgetRenderData FromModel(SitePageWidgetModel model) => new()
    {
        Id = model.Id,
        WidgetType = model.WidgetType,
        SettingsJson = model.SettingsJson,
        Row = model.Row,
        Column = model.Column,
        ColumnSpan = model.ColumnSpan,
        CssClass = model.CssClass,
        HtmlId = model.HtmlId,
        CustomAttributes = model.CustomAttributes
    };
}

#endregion

