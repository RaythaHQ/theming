# Raytha Theming – Cursor Setup

This repository is a local development workspace for Raytha templates with a built-in simulator.

**Key principle:** Write Liquid templates, generate HTML previews via the simulator.

---

## Directory Structure

```
├── liquid/                    # Liquid templates (PRIMARY FOCUS)
├── models/                    # Content type definitions (Markdown)
├── src/
│   ├── sample-data/           # Sample data JSON files
│   └── ...                    # Simulator source code
├── html/                      # Generated HTML output (DO NOT EDIT)
└── .cursor/
    └── setup.md               # This file
```

---

## Workflow

### 1. Define Content Models

Content models live in `/models/*.md` and define the structure of content types.

Format:
```markdown
# Articles Content Type

| Label       | Developer Name | Field Type        |
|-------------|----------------|-------------------|
| Title       | title          | single_line_text  |
| Summary     | summary        | long_text         |
| Content     | content        | wysiwyg           |
| Category    | category       | dropdown          |
| Difficulty  | difficulty     | radio             |
```

When creating or updating models, use these field types:
- `single_line_text` — Short text
- `long_text` — Multi-line text
- `wysiwyg` — Rich HTML content
- `dropdown` — Single select with choices
- `radio` — Radio button selection
- `multiple_select` — Multi-select
- `checkbox` — Boolean
- `date` — Date picker
- `number` — Numeric
- `attachment` — File upload
- `one_to_one_relationship` — Link to another content item

### 2. Generate Sample Data

Sample data JSON files in `/src/sample-data/` drive the simulator. Generate them based on the models.

**Structure for list views:**
```json
{
  "liquid_file": "raytha_html_content_item_list",
  "Target": {
    "Label": "Articles",
    "RoutePath": "articles.html",
    "Items": [
      {
        "detail_liquid_file": "raytha_html_content_item_detail",
        "PrimaryField": "Getting Started",
        "RoutePath": "articles_getting-started.html",
        "CreationTime": "2024-01-15T10:00:00Z",
        "PublishedContent": {
          "title": { "Text": "Getting Started", "Value": "Getting Started" },
          "content": { "Text": "<p>Article content...</p>", "Value": "<p>Article content...</p>" }
        }
      }
    ],
    "TotalCount": 1,
    "PageNumber": 1,
    "PageSize": 25,
    "TotalPages": 1
  },
  "ContentType": {
    "LabelPlural": "Articles",
    "LabelSingular": "Article",
    "DeveloperName": "articles"
  },
  "CurrentOrganization": { "OrganizationName": "My Site" },
  "CurrentUser": { "IsAuthenticated": false },
  "PathBase": ".",
  "QueryParams": {}
}
```

**Key rules:**
- `liquid_file` — Template used for list/main view
- `detail_liquid_file` — Per-item, specifies template for detail page
- `RoutePath` — Output HTML filename (e.g., `articles_getting-started.html`)
- `PathBase` — Use `"."` for relative links in simulator
- Fields use `{ "Text": "...", "Value": "..." }` format

**Menus** go in `menus.json`:
```json
{
  "menus": [
    {
      "DeveloperName": "main",
      "IsMainMenu": true,
      "MenuItems": [
        { "Label": "Home", "Url": "home.html" },
        { "Label": "Articles", "Url": "articles.html" }
      ]
    }
  ]
}
```

### 3. Write Liquid Templates

Templates live in `/liquid/*.liquid`. Focus your editing here.

**Template inheritance:**
Use `{% layout 'template_name' %}` at the top of child templates:
```liquid
{% layout 'raytha_html_base_layout' %}
<div class="container">
  {{ Target.PrimaryField }}
</div>
```

Parent templates use `{% renderbody %}` to inject child content.

**Available variables:**
- `Target` — Main content (single item or list with `.Items`)
- `Target.PrimaryField` — Primary field value
- `Target.PublishedContent.<field>.Text` — Field display value
- `Target.PublishedContent.<field>.Value` — Field raw value
- `Target.RoutePath` — Route path
- `Target.CreationTime` — Creation timestamp
- `ContentType.LabelPlural`, `ContentType.DeveloperName`
- `CurrentOrganization.OrganizationName`
- `CurrentUser.IsAuthenticated`, `CurrentUser.IsAdmin`
- `PathBase` — Base path for URLs
- `QueryParams` — URL query parameters

**Available functions:**
- `get_main_menu()` — Returns main navigation menu
- `get_menu('developer_name')` — Returns specific menu
- `get_content_items(ContentType='name', Filter='odata filter exp', OrderBy='developer_name asc/desc', PageNumber=1, PageSize=10)` — Query content
- `get_content_item_by_id(id)` — Get single item
- `get_content_type_by_developer_name(name)` — Get content type metadata

**Available filters:**
- `attachment_redirect_url` — Secure attachment URL
- `attachment_public_url` — Public attachment URL
- `organization_time` — Convert to org timezone
- `groupby: 'field'` — Group items by field
- `json` — Output as JSON (debugging)

### 4. Run the Simulator

```bash
cd src
dotnet run
```

This:
1. Reads all JSON files in `/src/sample-data/` (except `menus.json`)
2. Renders each using its `liquid_file` template
3. For items with `detail_liquid_file`, generates individual detail pages
4. Outputs HTML to `/html/`

### 5. Preview and Iterate

Open generated HTML files in a browser. All links should work locally.

To make changes:
1. Edit Liquid templates in `/liquid/`
2. Re-run `dotnet run` in `/src/`
3. Refresh browser

### 6. Deploy to Raytha

Copy final Liquid templates from `/liquid/` into Raytha's admin template editor.

---

## Important Rules

### DO:
- Focus editing on `/liquid/*.liquid` files
- Generate sample data based on `/models/*.md` definitions
- Use CDN links for CSS/JS (Bootstrap, etc.)
- Use `PathBase` of `"."` in sample data for relative links
- Set `RoutePath` on items to valid HTML filenames
- Include `detail_liquid_file` on items that need detail pages

### DO NOT:
- Edit files in `/html/` directly — they are generated
- Use Liquid syntax that the Fluid engine doesn't support
- Include actual Liquid `{{ }}` or `{% %}` in content text (it will be parsed)

---

## CSS and JavaScript

Always use CDN links in Liquid templates:

```liquid
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
```

---

## Image Handling

The simulator returns placeholder images for attachment filters:
- Images: `https://placehold.co/400x300/e2e8f0/64748b?text=Image+Placeholder`
- Files: `https://placehold.co/200x200/f1f5f9/475569?text=File`

In sample data, just use any filename for attachment values.

---

## Cursor Behavior Summary

When working in this repo, Cursor should:

1. **Focus on Liquid templates** — Edit files in `/liquid/`
2. **Generate sample data from models** — Read `/models/*.md`, create JSON in `/src/sample-data/`
3. **Run the simulator** — Execute `cd src && dotnet run` to regenerate HTML
4. **Iterate until correct** — Update Liquid, regenerate, review HTML
5. **Never edit `/html/` directly** — These are generated outputs
