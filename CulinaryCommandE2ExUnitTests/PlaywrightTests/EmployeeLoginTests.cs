using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EmployeeLoginTests : PageTest
{
    [Test]
    public async Task EmployeeCanLoginSuccessfully()
    {
        await Page.GotoAsync("http://3.20.198.36/signin");
        await Page.GetByText("Cook").ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("employee@test.com");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("testing123");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        
        // Uncomment these lines to test navigation
        // await Page.GetByRole(AriaRole.Link, new() { Name = "My Tasks" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = "Inventory" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Button, new() { Name = "Profile employee@test.com" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = "Settings" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Button, new() { Name = "Profile employee@test.com" }).ClickAsync();
        // await Page.GetByRole(AriaRole.Link, new() { Name = "Home" }).ClickAsync();
    }
}
