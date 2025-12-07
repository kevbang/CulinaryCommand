# CulinaryCommand Unit Tests

This folder contains automated tests for the **CulinaryCommand** application. The goal of this test project is to:

- Verify that core features behave as expected.
- Catch regressions early when refactoring.
- Document intended behavior through executable examples.

The test project is a standard **.NET test project** that you can run from the command line or from within Visual Studio / VS Code.

---

## 1. Basic Overview & Usage

### 1.1. Project layout

`CulinaryCommandUnitTests/` roughly mirrors the structure of the main app:

- `Inventory/` – tests around inventory components, DTOs, entities, services, etc.
- `Components/` – tests for reusable Blazor / UI components.
- Additional folders may be added as the application grows.

Tests are grouped by feature or layer so you can quickly find the code under test.

### 1.2. Running the tests

From the repository root:

```bash
# Run all tests
 dotnet test

 # Run using solution file
 dotnet test CulinaryCommandApp/CulinaryCommand.sln

# Run only this test project
 dotnet test CulinaryCommandUnitTests/CulinaryCommandUnitTests.csproj

# Run tests with more detailed output
 dotnet test -v normal
```

In VS Code you can use the **.NET Test Explorer** extension or run `dotnet test` in the integrated terminal.

---

## 2. Testing Tools & What They Are Used For

This project uses common .NET testing tools. Below is a quick guide to what each one is for and how it is typically used here.

> Note: The exact package list may change over time. Check the `CulinaryCommandUnitTests.csproj` file for the definitive set of test dependencies.

### 2.1. xUnit – Test Framework

**What it is:**

- xUnit is the primary test framework.
- It provides attributes and assertions to define and run tests.

**Typical usage:**

- Mark a test class:
  ```csharp
  public class InventoryServiceTests
  {
      // ...
  }
  ```
- Mark a single test method:
  ```csharp
  [Fact]
  public void createingredient_should_set_default_values()
  {
      // Arrange
      // Act
      // Assert
  }
  ```
- Assertions:
  ```csharp
  Assert.Equal(expected, actual);
  Assert.NotNull(result);
  Assert.True(condition);
  ```

We use **`[Fact]`** for single-scenario tests and **`[Theory]`** (see below) when the same test logic should run with different inputs.

### 2.2. Data‑driven tests with xUnit `[Theory]` + `InlineData`

**What it is:**

- xUnit supports **data‑driven tests** where the same test method runs multiple times with different input values.
- This is useful for testing DTO validation rules, mapping behavior, and small pure functions.

**Typical usage for DTO tests:**

```csharp
[Theory]
[InlineData("", false)]          // Name is required
[InlineData("Tomato", true)]     // Valid name
public void CreateIngredientDto_Name_Validation(string name, bool isValid)
{
    // Arrange
    var dto = new CreateIngredientDTO { Name = name };

    // Act
    var result = Validate(dto); // e.g., using DataAnnotations or custom validation

    // Assert
    Assert.Equal(isValid, result.IsValid);
}
```

Where appropriate, you can also use custom data attributes or member data to supply more complex input sets.

### 2.3. Data Annotations – DTO Validation

**What it is:**

- Many DTOs and models in the main project use **`System.ComponentModel.DataAnnotations`** attributes such as `[Required]`, `[StringLength]`, `[Range]`, etc.
- Tests in this project can exercise these attributes to ensure validation rules behave as expected.

**Typical usage pattern in tests:**

```csharp
var context = new ValidationContext(dto, serviceProvider: null, items: null);
var results = new List<ValidationResult>();

bool isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

Assert.True(isValid);
```

We generally use data‑driven tests (xUnit `[Theory]`) to feed multiple combinations of values into DTOs and assert whether they pass or fail validation.

### 2.4. Additional tools (if/when added)

Depending on future needs, the test project may add:

- **Mocking framework** (e.g., Moq, NSubstitute) –
  - For isolating services from infrastructure (DB, email, external APIs).
- **FluentAssertions** –
  - For more expressive assertions (`result.Should().NotBeNull();`).
- **Snapshot testing or bUnit** –
  - For testing Blazor components and UI behavior.

Check the `.csproj` file and individual test files to see which of these are currently in use.

---

## 3. How to Read & Write Tests

### 3.1. Naming conventions

- **Test class names** generally follow `*Tests` (e.g., `CompanyServiceTests`, `CreateIngredientDtoTests`).
- **Test method names** usually follow a pattern like:
  - `methodname_should_dosomething_when_condition()`
  - `scenario_expectedbehavior()`

This makes it easy to see what behavior is covered.

> Note: In C#, test method names typically follow a pattern like `MethodName_Should_DoSomething_When_Condition()`, etc. However, I prefer the method above. This can be changed.

### 3.2. Arrange / Act / Assert pattern

Most tests follow **AAA**:

1. **Arrange** – Set up inputs, dependencies, and state.
2. **Act** – Call the method or component under test.
3. **Assert** – Verify that the outcome matches expectations.

Example:

```csharp
[Fact]
public void createingredient_should_set_isactive_to_true_by_default()
{
    // Arrange
    var dto = new CreateIngredientDTO { Name = "Tomato" };

    // Act
    var entity = Map(dto); // e.g., mapping from DTO to entity

    // Assert
    Assert.True(entity.IsActive);
}
```

---

## 4. Contributing 101 – How to Add or Update Tests

This section is a quick start guide if you want to contribute tests to this project.

### 4.1. Prerequisites

- .NET SDK installed (check `global.json` in the repo for the recommended version).
- Ability to run `dotnet test` successfully before and after your changes.

### 4.2. General process

1. **Create or pick an issue / feature**
   - Identify the area you are testing (e.g., Inventory DTOs, Company services, Blazor components).

2. **Locate or create the matching test file**
   - For a class `CreateIngredientDTO` in the main app, there should ideally be a matching test file like:
     - `Inventory/DTOs/CreateIngredientDTOTests.cs` (or similar).
   - If it does not exist yet, create a new test file under the appropriate folder and namespace.

3. **Write focused tests**
   - Prefer several **small, focused tests** over one very large test.
   - Cover:
     - Happy-path behavior.
     - Common invalid inputs / edge cases.
     - Any bug you are fixing (with a regression test).

4. **Run tests locally**
   - From the repo root:
     ```bash
     dotnet test
     ```
   - Ensure all tests pass.

5. **Keep tests deterministic**
   - Avoid depending on the current time, external services, random values, or shared global state.
   - If needed, wrap those dependencies in abstractions and mock them.

6. **Submit your changes**
   - Follow the repository’s contribution workflow (branch naming, PR guidelines, etc.).
   - In your PR description, mention:
     - What area you tested.
     - Any new behavior you added coverage for.
     - Any bug/issue the tests guard against.

### 4.3. Style tips

- Prefer **clear test names** over short ones.
- Use **data‑driven tests** when validating many input/output combinations.
- Keep test setup minimal – use helper methods or builders if setup becomes repetitive.
- When in doubt, mirror the existing style of nearby tests.

---

## 5. Where to Go Next

- Browse the existing test files in this folder to see concrete examples.
- Check `CulinaryCommandApp/` to understand the production code being tested.

This document is intentionally introductory; feel free to extend it as the test suite and tooling evolve.
