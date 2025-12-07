import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://3.20.198.36/signin');
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('admin@test.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('testing1234');
  await page.getByRole('button', { name: 'Sign in' }).click();
  // await page.getByRole('link', { name: 'Inventory' }).click();
  // await page.getByRole('link', { name: ' Recipes' }).click();
  // await page.getByRole('link', { name: ' Ingredients' }).click();
  // await page.getByRole('link', { name: ' Manage Users' }).click();
  // await page.getByRole('link', { name: 'Home' }).click();
});