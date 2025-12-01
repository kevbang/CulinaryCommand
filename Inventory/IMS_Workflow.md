# Inventory Management System (IMS) Workflow

This document describes the process I followed (and plan to follow) to implement the first iteration of the Inventory Management System (IMS) in CulinaryCommand. The goal of this phase is simple:

> From the web app, I can navigate to an `/ingredients` tab and use a basic, traditional inventory management UI to create and view ingredients.

Everything here is scoped just to that outcome.

---

## 1. Data Model Design (Domain-First)

I started by designing the core inventory schema in a technology-agnostic way. The schema lives in `Inventory/.copilot_instructions.md` and is the reference for the domain.

Key tables:

- **ingredient**
  - Represents each stocked item.
  - Fields: `id`, `name`, `category`, `unit_id`, `stock_quantity`, timestamps.
- **unit**
  - Defines units of measure and conversion to a base unit.
  - Fields: `id`, `abbreviation`, `name`, `conversion_factor`.
- **recipe**, **recipe_ingredient**, **recipe_reference**
  - Focused on recipe composition and sub-recipes.
  - I am not fully implementing recipe logic yet; they just inform how I design models and relationships.
- **inventory_transactions**
  - Designed to support auditing stock movements.
  - Not fully wired into the UI in this phase, but the schema informs how I model inventory changes.

The intent here was to separate **conceptual design** from implementation and make sure inventory stays modular and self-contained.

---

## 2. Modular Project Structure under `Inventory/`

Next, I refactored the project to make inventory concerns modular. Instead of sprinkling inventory code across the existing folders, I grouped everything under `Inventory/`:

- `Inventory/Entities/`
  - Entity classes that map to the new tables (`Ingredient`, `Unit`, and future recipe-related entities).
- `Inventory/Models/`
  - View models / DTOs tailored for the UI, e.g. `IngredientViewModel` used by Blazor pages.
- `Inventory/Data/`
  - Any inventory-specific data helpers or future repositories.
- `Inventory/Services/`
  - Service layer that will eventually encapsulate inventory operations.
- `Inventory/Components/` and `Inventory/Pages/`
  - Blazor components/pages specific to inventory features.

The guiding principle: **Inventory is a vertical slice** (entities + services + UI) that can evolve without tightly coupling to the rest of the app.

---

## 3. Wiring Entities into EF Core & Migrations

I integrated the new inventory entities into the existing `AppDbContext` so they participate in the main database.

Steps I took:

1. **Add DbSets to `AppDbContext`**
   - Added `DbSet<Ingredient>`, `DbSet<Unit>`, and any other relevant inventory entities.
   - Ensured relationships match the schema (e.g., `Ingredient` has a FK to `Unit`).

2. **Configure relationships and constraints**
   - Used model configuration (either via data annotations or `OnModelCreating`) to:
     - Set required properties.
     - Configure FK constraints.
     - Optionally seed lookup data (e.g., a few basic `Unit` rows) to support the UI.

3. **Create and apply migration**
   - Ran `dotnet ef migrations add MapInventoryIngredient` (already present in `Migrations/`).
   - Verified the generated migration aligns with the schema in `.copilot_instructions.md`.
   - Applied the migration to the dev DB to make sure the schema is physically present.

At this point the DB is ready to persist ingredients and units.

---

## 4. Basic Ingredients UI (Blazor Page)

For the first UI iteration, I built a very simple, traditional inventory-style page for managing ingredients.

### 4.1 Ingredients Page

Moved `Components/Pages/Ingredients/Ingredients.razor` -> `Inventory/Pages/Ingredients/Ingredients.razor`

- Route: `@page "/ingredients"` (only one page should own this route).
- Injects `AppDbContext` directly for now (I’ll refactor to a service later).
- Shows:
  - A table of existing ingredients (name, category, default unit).
  - A simple form for adding/editing an ingredient inline.

Behavior:

- On initialization:
  - Loads ingredients including their `Unit` navigation (`Include(i => i.Unit)`) and orders them by name.
  - Loads all available `Unit` records for use in a dropdown.
- When clicking **Add Ingredient**:
  - Switches to `editing = true` and initializes a new `IngredientViewModel`.
- When clicking **Edit** on a row:
  - Populates the view model from the selected `Ingredient` (id, name, category, unit).
- On **Save**:
  - If `IngredientId == 0`, creates a new `Ingredient` entity and adds it.
  - Otherwise, fetches the entity by id, updates fields, and saves changes.
  - Refreshes the list and exits edit mode.

This gives me a first-pass CRUD-like experience for ingredients.

### 4.2 Fixing Routing Ambiguity

During testing I hit a Blazor routing error:

> The following routes are ambiguous: '/ingredients' in '...Index' and '...Ingredients'

To fix this, I made sure **only one** component uses `@page "/ingredients"`.

- I kept a single canonical route for `/ingredients`.
- The other file (if needed) should be a pure component (no `@page`) or use a different URL (e.g., `/ingredients/list`).

This was important to avoid the blank white screen caused by unhandled route table exceptions.

---

## 5. Navigation & UX Integration

To make the IMS discoverable in the app, I integrated it into the existing navigation.

Steps:

1. **Add Nav Menu Item**
   - In the main nav (`Components/Layout/NavMenu.razor` or equivalent), I added a new link:
     - Label: "Ingredients" or "Inventory".
     - Target: `/ingredients`.

2. **Align with Existing Look & Feel**
   - Used the existing Bootstrap-based styles to keep the page consistent with the rest of the app.
   - Structured the page into:
     - Primary action (Add Ingredient) at the top.
     - Data table for existing items.
     - Simple, familiar form layout (labels + inputs + dropdown) resembling a typical back-office inventory tool.

3. **Route Guards / Auth (if needed)**
   - The intent is that only admins/managers see the ingredients tab.
   - To keep scope small, any detailed authorization is handled via existing mechanisms (e.g., existing role checks or layouts) and can be refined later.

---

## 6. Service Layer (Next Iteration Cleanup)

Right now, the `Ingredients.razor` page talks directly to `AppDbContext`. That’s acceptable for a first spike, but my plan is to move logic behind a service for testability and reuse.

Planned steps:

1. **Create `IIngredientService` and implementation**
   - Location: `Inventory/Services/IngredientService.cs`.
   - Responsibilities:
     - Get a list of ingredients with units.
     - Create/update ingredients from a view model or DTO.
     - In the future, record inventory transactions when stocks move.

2. **Register in DI**
   - In `Program.cs` (or wherever services are configured), register:
     - `services.AddScoped<IIngredientService, IngredientService>();`

3. **Refactor Blazor Page to use the service**
   - Replace direct `AppDbContext` injection with `IIngredientService`.
   - Keep the page focused on UI concerns (binding, event handlers, state), while the service owns data access and business rules.

This keeps the inventory feature easy to extend when we add transactions, stock adjustments, etc.

---

## 7. Testing & Validation

Finally, I validate the flow end-to-end:

1. **Local Testing**
   - Run the app locally (`dotnet watch run` or similar).
   - Navigate to `/ingredients`.
   - Verify:
     - Page loads without routing errors.
     - Existing ingredients (if any) show up correctly with their units.
     - I can add a new ingredient and see it persisted.
     - I can edit an ingredient and see the updates reflected in the table.

2. **Database Check**
   - Inspect the `Ingredients` and `Units` tables directly (via a DB tool) to confirm rows match what the UI shows.

3. **Deployment Sanity Check**
   - After deploying to Lightsail, hit the `/ingredients` route.
   - Confirm the same behavior as locally.

---

## 8. Next Steps (Beyond Phase 1)

This phase focuses just on a basic ingredients list. Future phases will:

- Implement `inventory_transactions` and wire stock adjustments when recipes are sold.
- Introduce more advanced filtering and search on the ingredients list.
- Add pagination or virtual scrolling for large inventories.
- Implement recipe-to-ingredient expansion and net stock subtraction based on the `recipe`, `recipe_ingredient`, and `recipe_reference` schema.

For now, the system gives us a clean, modular foundation with:
- Inventory-specific domain entities under `Inventory/`.
- Database schema in sync with EF Core migrations.
- A working `/ingredients` page that feels like a simple, traditional inventory UI.
