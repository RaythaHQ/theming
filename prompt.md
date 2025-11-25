# Sample Prompt: Knowledge Base Website

This is an example prompt for building a knowledge base website using the Raytha template workflow. Modify it for your own project needs.

---

You are working in a Raytha theme repository. Follow the workflow in `.cursor/setup.md`:
1. Define content models in `/models/`
2. Generate sample data in `/src/sample-data/`
3. Write Liquid templates in `/liquid/`
4. Run the simulator to generate HTML previews
5. Iterate until the design is correct

---

## Goal

Design a **modern knowledge base / help center** for a SaaS product with:
- Clean, professional design with strong accent colors
- Responsive layout
- Good typography and visual hierarchy

---

## Step 1: Create the Content Model

Create `models/articles.md` with fields for knowledge base articles:

| Label        | Developer Name | Field Type        |
|--------------|----------------|-------------------|
| Title        | title          | single_line_text  |
| Summary      | summary        | long_text         |
| Content      | content        | wysiwyg           |
| Category     | category       | dropdown          |
| Difficulty   | difficulty     | radio             |

Categories might include: Getting Started, Integrations, Billing, Troubleshooting, API
Difficulty levels: Beginner, Intermediate, Advanced

---

## Step 2: Generate Sample Data

Create `/src/sample-data/articles.json` based on the model above.

Include:
- At least 5-6 realistic articles with varied categories and difficulties
- Each item should have `detail_liquid_file` set to generate individual article pages
- Use realistic SaaS help content (e.g., "Connecting Your Account", "API Authentication", "Managing Team Members")
- Set `RoutePath` for each item (e.g., `articles_connecting-your-account.html`)

Also update `menus.json` to include navigation to the articles list.

---

## Step 3: Create Liquid Templates

### Base Layout (`raytha_html_base_layout.liquid`)
Update the base layout with:
- Modern header with logo and navigation (use `get_main_menu()`)
- Clean footer
- Use Bootstrap 5 via CDN
- Add custom CSS for the knowledge base aesthetic

### Home Page (`raytha_html_home.liquid`)
Create a knowledge base landing page with:
- Hero section with search bar placeholder
- Category cards/tiles linking to filtered article lists
- "Popular Articles" or "Recently Updated" section
- Use `get_content_items(ContentType='articles', PageSize=6)` to fetch articles

### Article List (`raytha_html_content_item_list.liquid`)
Update for article listings:
- Page title showing the content type or category
- Search bar (can be non-functional placeholder)
- Article cards showing: title, category, summary, difficulty badge, last updated
- Pagination controls using `Target.PageNumber`, `Target.TotalPages`, etc.
- Loop through `Target.Items`

### Article Detail (`raytha_html_content_item_detail.liquid`)
Update for single article view:
- Breadcrumbs (Home > Category > Article Title)
- Article title and metadata (category, difficulty, last updated)
- Main content area with good typography for docs
- "Related Articles" section at the bottom

---

## Step 4: Run and Preview

After creating templates and sample data:

```bash
cd src
dotnet run
```

Open the generated HTML files in `/html/` to preview:
- `home.html`
- `articles.html` (list view)
- Individual article pages

---

## Step 5: Iterate

Review the HTML output. If changes are needed:
1. Update the Liquid templates in `/liquid/`
2. Re-run `dotnet run`
3. Refresh browser to see changes

Repeat until the design looks polished.

---

## Design Guidelines

- **Colors**: Choose a cohesive palette with primary accent, secondary accent, and neutral backgrounds
- **Typography**: Clear hierarchy with distinct heading sizes, readable body text
- **Cards**: Clean article preview cards with subtle shadows or borders
- **Badges**: Category and difficulty indicators as colored pills/badges
- **Spacing**: Generous whitespace, consistent padding
- **Responsive**: Works well on desktop and mobile

---

## When Complete

Once satisfied with the templates:
1. Copy the Liquid files from `/liquid/` into Raytha's template editor
2. Create the Articles content type in Raytha matching your model
3. Add real content and publish

---

## Notes

- The simulator uses placeholder images for attachments automatically
- All links between pages work when previewing locally
- Sample data should feel realistic to properly test the templates
- Focus on the Liquid templates — the HTML files are just for preview
