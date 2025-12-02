![rsz_color_logo_with_background](https://user-images.githubusercontent.com/777005/210120197-61101dee-91c7-4628-8fb4-c0d701843704.png)

Raytha documentation is available at https://raytha.com/docs

Raytha is a versatile and lightweight general purpose content management system. Create any type of website by easily configuring custom content types and HTML templates that can be directly edited within the platform.

[![Deploy on Railway](https://railway.com/button.svg)](https://railway.com/deploy/raytha-cms?referralCode=RU52It&utm_medium=integration&utm_source=template&utm_campaign=generic)

---

# Raytha Template Development with Cursor

This repository provides a reusable workflow for designing and building Raytha Liquid templates using Cursor, with a built-in simulator that renders your templates to HTML for preview.

## Project Structure

```
/raytha-theming/
├── src/                          # Simulator source (never modify)
│   ├── liquid/                   # Starter templates (copied to new projects)
│   │   ├── raytha_html_*.liquid  # Page templates
│   │   └── widgets/              # Widget templates
│   ├── models/                   # Starter content type definitions
│   │   └── posts.md
│   ├── Program.cs                # Simulator entry point
│   ├── RenderEngine.cs           # Liquid rendering engine
│   └── ...
├── dist/                         # YOUR PROJECTS (gitignored)
│   └── <site-name>/              # Each site is a separate project
│       ├── liquid/               # Customized templates
│       │   ├── raytha_html_*.liquid
│       │   └── widgets/
│       ├── sample-data/          # Your site's JSON data
│       │   ├── menus.json
│       │   ├── site-pages.json
│       │   └── posts.json
│       ├── models/               # Content type definitions
│       └── htmlOutput/           # Generated HTML
├── raytha.config.json            # API sync configuration
└── README.md
```

## Quick Start

### Step 1 — Initialize a New Project

```bash
cd src
dotnet run -- --site mywebsite
```

This creates `/dist/mywebsite/` with:
- Starter templates copied from `/src/liquid/`
- Empty `sample-data/` folder for your data
- Starter content model (`posts.md`)

### Step 2 — Add Sample Data

Create JSON files in `/dist/mywebsite/sample-data/`:

**menus.json** - Navigation menus (auto-generated on init)
**site-pages.json** - Site pages with widgets
**posts.json** (or other content types) - Content type data

### Step 3 — Customize Templates

Edit the Liquid templates in `/dist/mywebsite/liquid/`:
- Page templates: `raytha_html_*.liquid`
- Widget templates: `widgets/*.liquid`

### Step 4 — Render HTML

```bash
dotnet run -- --site mywebsite
```

Generated HTML files appear in `/dist/mywebsite/htmlOutput/`

### Step 5 — Preview and Iterate

Open the HTML files in your browser. Make changes to templates, re-run the simulator, and refresh.

---

## Commands

```bash
# Initialize or render a project
dotnet run -- --site <name>

# Render the default project
dotnet run

# Render and sync to Raytha
dotnet run -- --site mywebsite --sync

# Only sync templates (no render)
dotnet run -- --site mywebsite --sync-only

# Custom output directory
dotnet run -- --site mywebsite -o /path/to/output

# Show help
dotnet run -- --help
```

---

## Sample Data Files

### menus.json

Navigation menus used by templates:

```json
{
  "menus": [
    {
      "Id": "main-menu",
      "Label": "Main Menu",
      "DeveloperName": "main_menu",
      "IsMainMenu": true,
      "MenuItems": [
        { "Id": "1", "Label": "Home", "Url": "/", "Ordinal": 1 },
        { "Id": "2", "Label": "About", "Url": "/about", "Ordinal": 2 }
      ]
    }
  ]
}
```

### site-pages.json

Site pages with widget sections:

```json
{
  "CurrentOrganization": { "OrganizationName": "My Site", "TimeZone": "UTC" },
  "PathBase": "",
  "pages": [
    {
      "id": "home",
      "title": "Home",
      "routePath": "home",
      "webTemplateDeveloperName": "raytha_html_home",
      "publishedWidgets": {
        "hero": [
          {
            "id": "widget-1",
            "widgetType": "hero",
            "row": 1,
            "column": 1,
            "columnSpan": 12,
            "settingsJson": "{\"headline\": \"Welcome\", \"subheadline\": \"...\"}"
          }
        ]
      }
    }
  ]
}
```

### Content Type Files (e.g., posts.json)

Content items with list and detail views:

```json
{
  "liquid_file": "raytha_html_content_item_list",
  "ContentType": { "LabelPlural": "Posts", "DeveloperName": "posts" },
  "Target": {
    "Items": [
      {
        "detail_liquid_file": "raytha_html_content_item_detail",
        "PrimaryField": "My Post",
        "RoutePath": "posts_my-post",
        "PublishedContent": { "content": { "Text": "..." } }
      }
    ]
  }
}
```

---

## Widget Templates

Widget templates live in `liquid/widgets/` and are rendered via `render_section()`:

| Widget | File | Description |
|--------|------|-------------|
| Hero | `hero.liquid` | Large banner with headline and CTA |
| WYSIWYG | `wysiwyg.liquid` | Rich text content block |
| CTA | `cta.liquid` | Call-to-action section |
| Card | `card.liquid` | Single card with image and button |
| Image+Text | `imagetext.liquid` | Image alongside text content |
| FAQ | `faq.liquid` | Expandable accordion |
| Embed | `embed.liquid` | iframe or HTML embed |
| Content List | `contentlist.liquid` | Dynamic content type listing |

### Using Widgets in Templates

```liquid
{{ render_section("hero") }}
{{ render_section("features") }}
```

---

## Raytha API Sync

Sync templates to a running Raytha instance.

### Configuration

Create `raytha.config.json` in the project root:

```json
{
  "baseUrl": "http://localhost:5000",
  "apiKey": "YOUR_API_KEY_HERE",
  "themeDeveloperName": "raytha_default_theme",
  "autoSync": false
}
```

### Sync Commands

```bash
# Render and sync
dotnet run -- --site mywebsite --sync

# Only sync (no render)
dotnet run -- --site mywebsite --sync-only
```

### Important: Local-Only Syntax

The `{% layout 'name' %}` tag and `.html` in URLs are automatically stripped when syncing:

```liquid
{% layout 'raytha_html_base_layout' %}  → Stripped (parent sent via API)
href="{{ PathBase }}/about.html"        → href="{{ PathBase }}/about"
```

---

## Multiple Projects

Create separate sites for different projects:

```bash
dotnet run -- --site client-a
dotnet run -- --site client-b
dotnet run -- --site portfolio
```

Each lives in its own folder under `/dist/` with independent templates and data.

---

## Cursor AI Integration

The `.cursor/` folder contains instructions for Cursor AI to:
- Generate sample data based on your site description
- Create and customize Liquid templates
- Define content type models
- Help with widget configurations

Simply describe your website and let Cursor help you build it!
