import { test, expect } from '@playwright/test'

test.describe('Navigation', () => {
  test('nav links navigate between pages', async ({ page }) => {
    await page.goto('/')
    await expect(page.locator('h2')).toHaveText('Dashboard')

    await page.click('a[href="/products"]')
    await expect(page.locator('h2')).toHaveText('Products')

    await page.click('a[href="/users"]')
    await expect(page.locator('h2')).toHaveText('Users')

    await page.click('a[href="/orders"]')
    await expect(page.locator('h2')).toHaveText('Orders')

    await page.click('a[href="/"]')
    await expect(page.locator('h2')).toHaveText('Dashboard')
  })
})
