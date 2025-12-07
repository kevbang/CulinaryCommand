import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://3.20.198.36/signin');
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('manager@test.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('testing123');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await page.getByRole('link', { name: 'My Tasks' }).click();
  await page.getByRole('navigation').locator('div').filter({ hasText: 'My Tasks' }).click();
  await page.getByRole('link', { name: 'Inventory', exact: true }).click();
  await page.locator('div').filter({ hasText: 'Inventory' }).nth(3).click();
  await page.getByRole('link', { name: ' Recipes' }).click();
  await page.getByRole('link', { name: ' Ingredients' }).click();
  await page.getByRole('link', { name: ' Inventory' }).click();
  await page.getByRole('link', { name: 'Home' }).click();
  await page.getByRole('button', { name: 'Profile manager@test.com' }).click();
});