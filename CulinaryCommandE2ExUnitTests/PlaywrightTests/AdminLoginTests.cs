using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AdminLoginTests : PageTest
{
    [Test]
    public async Task AdminCanLoginSuccessfully()
    {
        await Page.GotoAsync("http://3.20.198.36/signin");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("admin@test.com");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("testing1234");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        
        // Uncomment these lines to test navigation
        // await Page.GetByRole(AriaRole.Link, new() { Name = "Inventory" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = " Recipes" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = " Ingredients" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = " Manage Users" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
    }
}
