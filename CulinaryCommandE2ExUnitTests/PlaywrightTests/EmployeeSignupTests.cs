using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class EmployeeSignupTests : PageTest
{
    [Test]
    public async Task EmployeeCanSignUpSuccessfully()
    {
        await Page.GotoAsync("http://3.20.198.36/signin");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Sign up" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First name" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First name" }).FillAsync("Employee");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First name" }).PressAsync("Tab");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last name" }).FillAsync("Test");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last name" }).PressAsync("Tab");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email" }).FillAsync("employee@test.com");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Enter your email" }).PressAsync("Tab");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Create a password" }).FillAsync("testing123");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Create a password" }).PressAsync("Tab");
        await Page.GetByRole(AriaRole.Button).First.PressAsync("Tab");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm your password" }).FillAsync("testing123");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign Up" }).ClickAsync();
    }
}
