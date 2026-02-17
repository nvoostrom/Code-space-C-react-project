import { render, screen, waitFor } from '@testing-library/react'
import Dashboard from './Dashboard'

beforeEach(() => {
  vi.restoreAllMocks()
})

describe('Dashboard', () => {
  it('renders stats after successful API call', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: () => Promise.resolve({
        productCount: 50,
        userCount: 30,
        orderCount: 100,
        totalRevenue: 12345.67,
        pendingOrders: 10,
        deliveredOrders: 60,
        cancelledOrders: 5,
      }),
    } as Response)

    render(<Dashboard />)

    expect(screen.getByText('Loading...')).toBeInTheDocument()

    await waitFor(() => {
      expect(screen.getByText('50')).toBeInTheDocument()
    })

    expect(screen.getByText('30')).toBeInTheDocument()
    expect(screen.getByText('100')).toBeInTheDocument()
    expect(screen.getByText('10')).toBeInTheDocument()
    expect(screen.getByText('60')).toBeInTheDocument()
    expect(screen.getByText('5')).toBeInTheDocument()
  })

  it('renders error state on API failure', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: false,
      status: 500,
    } as Response)

    render(<Dashboard />)

    await waitFor(() => {
      expect(screen.getByText('Error: API error: 500')).toBeInTheDocument()
    })
  })
})
