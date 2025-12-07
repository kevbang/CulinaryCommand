import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://3.20.198.36/signin');
  await page.getByText('Cook').click();
  await page.getByRole('textbox', { name: 'Email' }).click();
  await page.getByRole('textbox', { name: 'Email' }).fill('employee@test.com');
  await page.getByRole('textbox', { name: 'Email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('testing123');
  await page.getByRole('button', { name: 'Sign in' }).click();
  // await page.getByRole('link', { name: 'My Tasks' }).click();
  // await page.getByRole('link', { name: 'Inventory' }).click();
  // await page.getByRole('button', { name: 'Profile employee@test.com' }).click();
  // await page.getByRole('link', { name: 'Settings' }).click();
  // await page.getByRole('button', { name: 'Profile employee@test.com' }).click();
  // await page.getByRole('link', { name: 'Home' }).click();
});