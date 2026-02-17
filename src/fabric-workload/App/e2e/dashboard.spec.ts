import { test, expect } from '@playwright/test'

test.describe('Dashboard', () => {
  test('loads and shows stat cards', async ({ page }) => {
    await page.goto('/')
    await expect(page.locator('h2')).toHaveText('Dashboard')

    // Wait for stats to load (loading state disappears)
    await expect(page.locator('.stat-card')).toHaveCount(7, { timeout: 10000 })

    // Verify stat labels are present
    await expect(page.getByText('Products')).toBeVisible()
    await expect(page.getByText('Users')).toBeVisible()
    await expect(page.getByText('Orders')).toBeVisible()
    await expect(page.getByText('Total Revenue')).toBeVisible()
  })
})
