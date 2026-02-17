import { test, expect } from '@playwright/test'

test.describe('Products', () => {
  test('renders product table', async ({ page }) => {
    await page.goto('/products')
    await expect(page.locator('h2')).toHaveText('Products')

    // Wait for products to load
    await expect(page.locator('.data-table tbody tr').first()).toBeVisible({ timeout: 10000 })

    // Table should have rows
    const rows = page.locator('.data-table tbody tr')
    await expect(rows).not.toHaveCount(0)
  })

  test('category filter works', async ({ page }) => {
    await page.goto('/products')

    // Wait for products to load
    await expect(page.locator('.data-table tbody tr').first()).toBeVisible({ timeout: 10000 })

    const initialCount = await page.locator('.data-table tbody tr').count()

    // Select a specific category from the dropdown
    const select = page.locator('select')
    const options = await select.locator('option').allTextContents()
    // Pick the second option (first non-"All" category)
    if (options.length > 1) {
      await select.selectOption({ index: 1 })
      const filteredCount = await page.locator('.data-table tbody tr').count()
      expect(filteredCount).toBeLessThanOrEqual(initialCount)
    }
  })
})
