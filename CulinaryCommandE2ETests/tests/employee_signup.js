import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://3.20.198.36/signin');
  await page.getByRole('link', { name: 'Sign up' }).click();
  await page.getByRole('textbox', { name: 'First name' }).click();
  await page.getByRole('textbox', { name: 'First name' }).fill('Employee');
  await page.getByRole('textbox', { name: 'First name' }).press('Tab');
  await page.getByRole('textbox', { name: 'Last name' }).fill('Test');
  await page.getByRole('textbox', { name: 'Last name' }).press('Tab');
  await page.getByRole('textbox', { name: 'Enter your email' }).fill('employee@test.com');
  await page.getByRole('textbox', { name: 'Enter your email' }).press('Tab');
  await page.getByRole('textbox', { name: 'Create a password' }).fill('testing123');
  await page.getByRole('textbox', { name: 'Create a password' }).press('Tab');
  await page.getByRole('button').first().press('Tab');
  await page.getByRole('textbox', { name: 'Confirm your password' }).fill('testing123');
  await page.getByRole('button', { name: 'Sign Up' }).click();
});