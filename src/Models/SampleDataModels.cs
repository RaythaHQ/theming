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

