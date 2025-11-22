You are working in a Raytha theme repo that follows the workflow and conventions described in `.cursor/setup.md` (HTML first, then Liquid, using `/models` for content types).

Goal: Design a small but **impressively well-designed knowledge base website** for a single SaaS product. The site should look like a modern docs / help center:

- Clean, professional, but with strong accent colors and good typography.
- Responsive layout.
- Minimal but thoughtful use of icons, spacing, and visual hierarchy.

### Overall constraints

- Follow the HTML→Liquid workflow in `.cursor/setup.md`.
- Use `/html` for fully rendered mockups (no Liquid, just static HTML).
- Use `/liquid` for Raytha Liquid templates that will eventually be copy/pasted into Raytha.
- Use `/models` to define and evolve the content models. You may modify or add model files as needed.
- For images in HTML mockups, use `https://placehold.co/{width}x{height}`.
- For CSS/JS libraries (e.g., Bootstrap), **use CDNs only**.
- Each HTML file must be a self-contained, valid HTML5 page that renders correctly on its own.

---

## 1. Content models

1. Create or update a model file for the knowledge base articles, e.g. `models/articles.md`, in the same simple table format as existing models.

   Use a **thin set of fields** that covers common KB needs, something like:

   - `title` – single_line_text
   - `slug` – single_line_text
   - `summary` – single_line_text
   - `content` – wysiwyg or long_text
   - `category` – single_line_text or single_select
   - `last_updated` – datetime
   - `difficulty` – single_select (e.g., Beginner / Intermediate / Advanced)

   Keep the model concise but realistic for a SaaS knowledge base.

2. If needed, lightly adjust `pages.md` or add another model (e.g. `categories.md`) but only if it clearly helps the design.

---

## 2. Layout and CSS

1. In `/html`, create a dedicated CSS file for the knowledge base, for example:

   - `html/knowledge_base.css`

2. Choose a **cohesive color system**:

   - Primary accent color (for buttons, links, key highlights)
   - Secondary accent color (badges, labels)
   - Neutral background + surface colors
   - Typographic scale (headings, body, small meta text)

3. Implement the CSS in `knowledge_base.css` with:

   - A simple, modern layout system (flex / grid is fine).
   - Nice card styles for article previews.
   - Good spacing and line-height defaults.
   - Styles for:
     - Top navigation / header
     - Search bar
     - Category pill/badges
     - Article list cards
     - Article content body
     - Breadcrumbs
     - Footer

4. Reference this CSS from each HTML page via a `<link>` tag.

You may optionally use Bootstrap via CDN **in addition to** or instead of custom CSS, but the end result should feel like a polished product docs site, not a raw default Bootstrap demo.

---

## 3. HTML pages to create

Create / update the following HTML mockups in `/html`:

### 3.1 Home page – `html/home.html`

Purpose: A landing page for the knowledge base.

Requirements:

- Hero section with:
  - Product name and short tagline about the SaaS.
  - Prominent search bar (“Search the docs…”).
- Highlighted key categories (e.g., Getting Started, Integrations, Billing, Troubleshooting) as visually appealing cards or tiles.
- A “Popular articles” or “Recently updated” section showing a few articles with:
  - Title
  - Category
  - Short summary
  - Last updated date
- Use **hardcoded static text** for most of this page; assume this rarely changes.
- Include header and footer that will make sense to reuse for list/detail pages.

Use fake content that fits a generic SaaS help center (e.g., “Connecting Your Account”, “Managing Team Members”, “API Authentication”, etc.).

### 3.2 Article listing page – `html/article_list.html`

Purpose: Show a list of knowledge base articles.

Requirements:

- Reuse the same header/nav and overall layout as the home page.
- Title area with page title, e.g. **“All Articles”** or a category name.
- Search bar (can be non-functional static HTML).
- Filters/labels (e.g., category pills, difficulty tags) represented visually, even if static.
- Article list as cards or rows. For each article, include:
  - Title
  - Category
  - Short summary
  - Last updated date
  - Difficulty (optional badge)
- Use simulated content based on the `articles` model defined in `/models/articles.md`.

### 3.3 Article detail page – `html/article_detail.html`

Purpose: Show a single knowledge base article.

Requirements:

- Reuse header/nav + footer.
- Breadcrumbs (e.g.: Home / Getting Started / Connecting Your Account).
- Article title, metadata row (category, last updated, difficulty).
- Main content area with realistic doc-style formatting:
  - Multiple headings
  - Paragraphs
  - Inline code samples
  - Numbered and bulleted lists
  - Callout / tip boxes (using CSS only, no JS needed).
- At the bottom, a “Related articles” section with a few article cards.
- All content should be static HTML that *resembles* real data from the `articles` model.

---

## 4. Integration with Liquid templates (planning only for now)

Do **not** modify the Liquid templates yet. Instead:

- Make sure the HTML structure and classes you build can be cleanly mapped later into the existing Liquid layouts:
  - `raytha_html_base_layout.liquid`
  - `raytha_html_content_item_detail.liquid`
  - `raytha_html_content_item_list.liquid`
- Keep in mind that:
  - The home page can map to a Raytha list view or a simple page template later.
  - `article_list.html` will map to a list view using `Target.Items`.
  - `article_detail.html` will map to a detail view using `Target`.

Once the HTML is complete and I confirm I’m happy with the look and structure, we will do a second pass where you:

- Update the Liquid templates under `/liquid` to match the approved HTML.
- Replace hardcoded content with the correct Liquid variables and loops (using `/models` as the source of truth).

---

## 5. First tasks to run now

1. Inspect `.cursor/setup.md` and the current `/models` and `/html` and `/liquid` directories.
2. Create or update `models/articles.md` with the thin article schema described above.
3. Propose a color system and overall visual style (briefly in comments or a short Markdown note).
4. Implement:
   - `html/knowledge_base.css`
   - `html/home.html`
   - `html/article_list.html`
   - `html/article_detail.html`
5. Make sure each HTML file:
   - References the CSS file,
   - Loads any CDN libraries it needs,
   - And renders as a fully functional standalone page.

After you’ve done this, show me a summary of the new/changed files and the overall layout so I can review the design before we move on to updating the Liquid templates.
