![rsz_color_logo_with_background](https://user-images.githubusercontent.com/777005/210120197-61101dee-91c7-4628-8fb4-c0d701843704.png)

Raytha documentation is available at https://raytha.com/docs

Raytha is a versatile and lightweight general purpose content management system. Create any type of website by easily configuring custom content types and HTML templates that can be directly edited within the platform.

[![Deploy on Railway](https://railway.com/button.svg)](https://railway.com/deploy/raytha-cms?referralCode=RU52It&utm_medium=integration&utm_source=template&utm_campaign=generic)

---

# Raytha Template Development with Cursor

This repository provides a workflow for designing and building Raytha Liquid templates using Cursor, with a built-in simulator that renders your templates to HTML for preview.

## How It Works

1. **Define content models** in `/models/*.md`
2. **Write Liquid templates** in `/liquid/*.liquid`
3. **Generate sample data** in `/src/sample-data/*.json` based on your models
4. **Run the simulator** to render HTML files to `/html/`
5. **Preview and iterate** until templates look right
6. **Copy Liquid templates** into your Raytha instance

## Repository Structure

```
├── liquid/                    # Liquid templates (your main focus)
│   ├── raytha_html_base_layout.liquid
│   ├── raytha_html_content_item_detail.liquid
│   ├── raytha_html_content_item_list.liquid
│   └── raytha_html_home.liquid
├── models/                    # Content type definitions
│   ├── posts.md
│   └── pages.md
├── src/                       # Template simulator (.NET 10)
│   ├── sample-data/           # Sample data JSON files
│   │   ├── posts.json
│   │   ├── pages.json
│   │   ├── home.json
│   │   └── menus.json
│   └── ...
├── html/                      # Generated HTML output (for preview)
└── .cursor/
    └── setup.md               # Cursor AI instructions
```

## Quick Start

### Step 1 — Deploy Raytha
Deploy Raytha using the Railway button above so you have a running instance.

### Step 2 — Open in Cursor
Clone this repository and open it in Cursor. The AI will understand the project structure via `.cursor/setup.md`.

### Step 3 — Define Your Content Models
Edit or create model files in `/models/` to define your content types:

```markdown
# Articles Content Type

| Label      | Developer Name | Field Type   |
|------------|----------------|--------------|
| Title      | title          | single_line_text |
| Content    | content        | wysiwyg      |
| Category   | category       | dropdown     |
```

### Step 4 — Generate Sample Data
Ask Cursor to generate sample data JSON files based on your models. Sample data goes in `/src/sample-data/` and includes:
- Content items with realistic fake data
- Navigation menus
- Organization and user context

### Step 5 — Write Liquid Templates
Create or edit Liquid templates in `/liquid/`. Use the `{% layout %}` tag for template inheritance:

```liquid
{% layout 'raytha_html_base_layout' %}
<div class="container">
  <h1>{{ Target.PrimaryField }}</h1>
  {{ Target.PublishedContent.content.Text }}
</div>
```

### Step 6 — Run the Simulator
```bash
cd src
dotnet run
```

This renders all sample data files to HTML in the `/html/` directory.

### Step 7 — Preview and Iterate
Open the generated HTML files in your browser. If changes are needed:
1. Update the Liquid templates
2. Re-run the simulator
3. Refresh your browser

### Step 8 — Deploy to Raytha
Once satisfied, you have two options:

**Option A: Manual Copy**
Copy your Liquid templates into Raytha's template editor.

**Option B: Auto-Sync via API** (Recommended)
Configure `raytha.config.json` to automatically sync templates to Raytha:

```bash
# Copy the example config
cp raytha.config.example.json raytha.config.json

# Edit and add your API key and theme developer name
```

Then run with the `--sync` flag:
```bash
cd src
dotnet run -- --sync      # Render and sync to Raytha
dotnet run -- --sync-only # Only sync (skip rendering)
```

---

## Raytha API Sync

The simulator can automatically sync your Liquid templates to a running Raytha instance via the API.

### Configuration

Create `raytha.config.json` in the project root (copy from `raytha.config.example.json`):

```json
{
  "baseUrl": "http://localhost:5000",
  "apiKey": "YOUR_API_KEY_HERE",
  "themeDeveloperName": "raytha_default_theme",
  "autoSync": false
}
```

### Configuration Options

| Option | Description |
|--------|-------------|
| `baseUrl` | Raytha instance URL (default: `http://localhost:5000`) |
| `apiKey` | Your Raytha API key (get from Admin → Settings → API Keys) |
| `themeDeveloperName` | Developer name of your theme (must already exist in Raytha) |
| `autoSync` | When `true`, sync automatically after every render |

### How Templates Are Discovered

Templates are **automatically discovered** from the `liquid/` directory:

- **Developer name** = filename without `.liquid` extension (e.g., `raytha_html_home.liquid` → `raytha_html_home`)
- **Parent layout** = extracted from `{% layout 'name' %}` tag in the file
- **Is base layout** = `true` if template contains `{% renderbody %}`
- **Label** = auto-generated from filename (e.g., `raytha_html_home` → "Raytha Html Home")

### Optional: Override Labels

If you want custom labels in Raytha admin, add a `templates` section to your config:

```json
{
  "baseUrl": "http://localhost:5000",
  "apiKey": "YOUR_API_KEY_HERE",
  "themeDeveloperName": "raytha_default_theme",
  "autoSync": false,
  "templates": {
    "raytha_html_home": {
      "label": "Homepage Template"
    }
  }
}
```

### Sync Commands

```bash
cd src
dotnet run -- --sync       # Render all templates, then sync to Raytha
dotnet run -- --sync-only  # Only sync templates (skip local rendering)
```

### ⚠️ Important: Local-Only Syntax

The following are **local-only** and automatically transformed when syncing:

| Local Syntax | Transformation | Reason |
|--------------|----------------|--------|
| `{% layout 'name' %}` | Stripped | Parent relationship sent via API instead |
| `.html` in URLs | Stripped | Local simulator uses `.html` files, Raytha uses clean URLs |

The `{% renderbody %}` tag works in **both** local simulator and production Raytha.

**Example transformations when syncing:**
```liquid
{% layout 'raytha_html_base_layout' %}     → (removed, parent sent via API)
href="{{ PathBase }}/{{ Type }}.html"      → href="{{ PathBase }}/{{ Type }}"
```

---

## Simulator Details

The simulator is a .NET 10 console app that uses the Fluid templating engine (same as Raytha).

### Features
- **Template inheritance** via `{% layout 'template_name' %}`
- **Custom Raytha filters**: `attachment_redirect_url`, `attachment_public_url`, `organization_time`, `groupby`, `json`
- **Custom functions**: `get_content_items()`, `get_main_menu()`, `get_menu()`, etc.
- **Automatic detail page generation** for each content item with `detail_liquid_file`

### Sample Data Format
Each JSON file specifies:
- `liquid_file` — which template to use for rendering
- `Target` — the main data object (list or single item)
- `ContentType`, `CurrentOrganization`, `CurrentUser`, `PathBase`

Items can specify `detail_liquid_file` to generate individual detail pages:

```json
{
  "liquid_file": "raytha_html_content_item_list",
  "Target": {
    "Items": [
      {
        "detail_liquid_file": "raytha_html_content_item_detail",
        "PrimaryField": "My Article",
        "RoutePath": "articles_my-article.html",
        ...
      }
    ]
  }
}
```

### Running the Simulator
```bash
cd src
dotnet run                    # Render all sample data files
dotnet run -- posts.json      # Render a specific file
dotnet run -- --help          # Show help
```

---

## Cursor AI Integration

The `.cursor/setup.md` file instructs Cursor how to work with this repository:
- Focus on Liquid templates in `/liquid/`
- Generate sample data from `/models/`
- Run the simulator to preview changes
- Use CDNs for CSS/JS (Bootstrap, etc.)
- Use placeholder images that work in the simulator

---

## Summary

1. Define content models in `/models/`
2. Generate sample data in `/src/sample-data/`
3. Write Liquid templates in `/liquid/`
4. Run `dotnet run` in `/src/` to generate HTML
5. Preview HTML files and iterate
6. Copy final Liquid templates to Raytha
