import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import Products from './Products'

const mockProducts = [
  { id: 1, name: 'Widget A', category: 'Electronics', price: 29.99 },
  { id: 2, name: 'Widget B', category: 'Electronics', price: 49.99 },
  { id: 3, name: 'Shirt C', category: 'Clothing', price: 19.99 },
]

beforeEach(() => {
  vi.restoreAllMocks()
  vi.spyOn(globalThis, 'fetch').mockResolvedValue({
    ok: true,
    json: () => Promise.resolve(mockProducts),
  } as Response)
})

describe('Products', () => {
  it('renders product table with data', async () => {
    render(<Products />)

    await waitFor(() => {
      expect(screen.getByText('Widget A')).toBeInTheDocument()
    })

    expect(screen.getByText('Widget B')).toBeInTheDocument()
    expect(screen.getByText('Shirt C')).toBeInTheDocument()
    expect(screen.getByText('$29.99')).toBeInTheDocument()
    expect(screen.getByText('3 products')).toBeInTheDocument()
  })

  it('filters products by category', async () => {
    const user = userEvent.setup()
    render(<Products />)

    await waitFor(() => {
      expect(screen.getByText('Widget A')).toBeInTheDocument()
    })

    await user.selectOptions(screen.getByRole('combobox'), 'Clothing')

    expect(screen.getByText('Shirt C')).toBeInTheDocument()
    expect(screen.queryByText('Widget A')).not.toBeInTheDocument()
    expect(screen.getByText('1 products')).toBeInTheDocument()
  })
})
