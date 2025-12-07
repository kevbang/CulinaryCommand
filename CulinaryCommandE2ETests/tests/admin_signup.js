import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://3.20.198.36/signin');
  await page.getByRole('link', { name: 'Create Restaurant' }).click();
  await page.locator('input[name="Signup.Company.Name"]').click();
  await page.locator('input[name="Signup.Company.Name"]').fill('Cyclone Cafe');
  await page.locator('input[name="Signup.Company.Address"]').click();
  await page.locator('input[name="Signup.Company.Address"]').fill('1234 Main St');
  await page.locator('input[name="Signup.Company.City"]').click();
  await page.locator('input[name="Signup.Company.City"]').fill('Ames');
  await page.locator('input[name="Signup.Company.State"]').click();
  await page.locator('input[name="Signup.Company.State"]').fill('IA');
  await page.locator('input[name="Signup.Company.ZipCode"]').click();
  await page.locator('input[name="Signup.Company.ZipCode"]').fill('12345');
  await page.locator('input[name="FirstName"]').click();
  await page.locator('input[name="FirstName"]').fill('Admin');
  await page.locator('input[name="LastName"]').click();
  await page.locator('input[name="LastName"]').fill('Test');
  await page.locator('input[name="Signup.Admin.Email"]').click();
  await page.locator('input[name="Signup.Admin.Email"]').fill('admin@test.com');
  await page.locator('input[name="Signup.Admin.Password"]').click();
  await page.locator('input[name="Signup.Admin.Password"]').fill('testing1234');
  await page.getByRole('button', { name: 'Create Account' }).click();
});