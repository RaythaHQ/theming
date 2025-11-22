# Raytha Theming – Cursor Setup

This repository is a local development workspace for Raytha templates.  
The goal is:

- Design and refine pages locally in **pure HTML**.
- Once the HTML looks correct, **translate it back** into **Liquid** templates Raytha can use.
- Keep the HTML and Liquid versions logically in sync, using `/models` as the source for fake data.

---

## Layout and template relationships

- Primary layout file (Liquid):

  - `liquid/raytha_html_base_layout.liquid`
  - This is the **main layout** that other templates inherit from.

- Primary content templates (Liquid):

  - `liquid/raytha_html_content_item_detail.liquid`
    - Detail view for a single content item.
    - Inherits from `raytha_html_base_layout.liquid`.
  - `liquid/raytha_html_content_item_list.liquid`
    - List view template for content items.
    - Inherits from `raytha_html_base_layout.liquid`.

Cursor should assume:

- `raytha_html_base_layout.liquid` defines the overall `<html>`, `<head>`, and shared layout.
- Detail and list templates plug their content into the layout via the usual Liquid conventions (`{% renderbody %}` etc.).

---

## HTML vs Liquid workflow

Directories:

- Liquid templates: `liquid/*.liquid`
- Rendered HTML mockups: `html/*.html`
- Content models: `models/*.md`

Workflow:

1. **Work in HTML first**

   - When I’m iterating on design or structure, **update the HTML files** in `/html`:
     - `html/home.html`
     - `html/page_detail.html`
     - `html/blog_list.html`
     - `html/blog_detail.html`
     - And any new HTML files we create.
   - HTML files must:
     - Contain **no Liquid syntax**.
     - Render correctly on their own in a browser.
     - Include full `<html>`, `<head>`, and `<body>` sections.
     - Use realistic **simulated data** instead of variables, based on the content models in `/models`.

2. **Use models for simulated data**

   - The files in `/models` (e.g. `models/pages.md`, `models/posts.md`) define:
     - Content type names
     - Field labels
     - Developer names
     - Field types
   - When generating or updating HTML mockups:
     - Use fake but plausible values that match these fields.
     - Reflect the shape of the data (dates, booleans, multi-selects, relationships, etc.).
   - Cursor is allowed to:
     - **Modify or extend** the Markdown files in `/models` when I describe new fields or content types.
     - Use those updated models when generating both HTML and Liquid.

3. **Then convert HTML back to Liquid**

   - Once I say I’m happy with the HTML for a page or template, the next step is to **update the corresponding Liquid files** in `/liquid`.
   - The process should be:
     - Start from the final HTML.
     - Carefully replace simulated data with the correct Liquid variables and control flow.
     - Keep the structure and semantics of the HTML as close as possible to the approved mockup.
   - Only update Liquid files when I explicitly give the go-ahead (e.g. “ok, now update the Liquid templates to match this HTML”).

---

## Image handling

- In **HTML mockups**, do **not** reference real image URLs.
- Use https://placehold.co instead.

Rules:

- Use `https://placehold.co/{width}x{height}` format.

Examples:

```html
<img src="https://placehold.co/600x400" alt="Placeholder image">
<img src="https://placehold.co/300x300" alt="Author avatar">
```

- In **Liquid templates**, convert these placeholders into the **appropriate Raytha variables**, e.g.:

```liquid
<img
  src="{{ Target.PublishedContent.featured_image.Value | attachment_redirect_url }}"
  alt="{{ Target.PublishedContent.title.Text }}"
>
```

---

## CSS and JS (Bootstrap & others)

- **Do not** embed local copies of Bootstrap or other libraries in the HTML.
- Always use **CDN links** for CSS and JS in HTML mockups.

Example:

```html
<!-- CSS -->
<link
  href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
  rel="stylesheet"
  integrity="sha384-..."
  crossorigin="anonymous"
/>

<!-- JS bundle (with Popper) -->
<script
  src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"
  integrity="sha384-..."
  crossorigin="anonymous"
></script>
```

HTML files should be **self-contained**:

- If a page relies on Bootstrap, include the required CDN links directly in that HTML file.
- Each HTML file must load everything it needs and should render cleanly by itself.

---

## Requirements for HTML files

For every `*.html` file in `/html`:

- No Liquid syntax.
- Valid HTML5:
  - `<html>`, `<head>`, `<body>` present.
- Use placeholders for:
  - Data from content types (fake static values).
  - Images (via placehold.co).
- Include any necessary CSS/JS via CDNs.
- Should render as a complete, working page when opened directly in a browser.

---

## Cursor behavior summary

- Prefer editing **HTML mockups** first when changing layout/design.
- Use `/models/*.md` to understand fields and simulate realistic data.
- Feel free to:
  - Add/edit Markdown model files when I describe new fields or content types.
  - Create new HTML and Liquid files when needed.
- After I approve the HTML version, generate/update the **corresponding Liquid templates** under `/liquid`, replacing simulated data with the correct Raytha Liquid variables and logic.