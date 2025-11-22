![rsz_color_logo_with_background](https://user-images.githubusercontent.com/777005/210120197-61101dee-91c7-4628-8fb4-c0d701843704.png)

Raytha documentation is available at https://raytha.com/docs

Raytha is a versatile and lightweight general purpose content management system. Create any type of website by easily configuring custom content types and HTML templates that can be directly edited within the platform.

[![Deploy on Railway](https://railway.com/button.svg)](https://railway.com/deploy/raytha-cms?referralCode=RU52It&utm_medium=integration&utm_source=template&utm_campaign=generic)

---

# Vibe Code Website Templates with Cursor into a CMS

This repository provides a workflow for designing and building Raytha templates using Cursor. The Knowledge Base theme included here is only an example. The same workflow applies to building any site, layout, or template structure for Raytha.

## How to use this repository

### Step 1 — Deploy Raytha
Deploy Raytha using the Railway button above so you have a running instance to paste templates into when ready.

### Step 2 — Open this repository in Cursor
Load or clone this repository into Cursor. Cursor will understand the project structure.

### Step 3 — Begin by prompting Cursor
Open `prompt.md` and modify or copy its contents into Cursor. It comes out of the box with a full fledged example for building a knowledge base website. 
This sets the expectations and explains how Cursor should behave.

### Step 4 — Work in HTML files first
Modify the HTML files in `/html` to design and iterate.  
This phase is for layout, styling, and structure.

Rules:
- Do not use Liquid syntax in HTML
- Each HTML file must render correctly on its own
- Use CDN-hosted CSS and JS
- Use https://placehold.co for image placeholders
- Cursor may create or update a CSS file if useful

### Step 5 — Approve the HTML
Continue updating the HTML until you are satisfied with how the pages look and feel.

### Step 6 — Convert to Liquid templates
After approval, instruct Cursor to:

- update the corresponding Liquid templates in `/liquid`
- replace simulated data with Raytha variables
- follow Raytha template conventions
- keep structure aligned with the approved HTML

### Step 7 — Copy Liquid into Raytha
Paste the updated Liquid templates into your Raytha instance using the admin template editor.

### Step 8 — Iterate whenever needed
If you want to change anything in the future:

1. update the HTML version first  
2. approve it  
3. prompt Cursor to update the Liquid  
4. paste into Raytha again  

### Step 9 — Content types
The `/models` directory contains Markdown definitions for content types.  
Cursor will reference these when generating simulated HTML data and Liquid bindings.

If content types change in Raytha:
- update the corresponding model file, or
- instruct Cursor to update it for you

---

## Summary of the workflow
1. Deploy Raytha  
2. Open this repo in Cursor  
3. Use the kickoff prompt  
4. Modify HTML mockups  
5. Approve the design  
6. Convert to Liquid  
7. Paste into Raytha  
8. Repeat when needed

---

This repository provides a practical way to design, preview, iterate, and generate Raytha templates without writing Liquid first. The Knowledge Base theme included is only a starting point; the same workflow applies to any template or site structure you want to build.
