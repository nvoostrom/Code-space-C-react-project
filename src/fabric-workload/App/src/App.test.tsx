import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import App from './App'

// Mock page components to avoid API calls
vi.mock('./pages/Dashboard', () => ({ default: () => <div>Dashboard Page</div> }))
vi.mock('./pages/Products', () => ({ default: () => <div>Products Page</div> }))
vi.mock('./pages/Users', () => ({ default: () => <div>Users Page</div> }))
vi.mock('./pages/Orders', () => ({ default: () => <div>Orders Page</div> }))

function renderApp(route = '/') {
  return render(
    <MemoryRouter initialEntries={[route]}>
      <App />
    </MemoryRouter>
  )
}

describe('App', () => {
  it('renders all nav links', () => {
    renderApp()
    expect(screen.getByRole('link', { name: 'Dashboard' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Products' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Users' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Orders' })).toBeInTheDocument()
  })

  it('renders Dashboard by default', () => {
    renderApp('/')
    expect(screen.getByText('Dashboard Page')).toBeInTheDocument()
  })

  it('renders Products page on /products', () => {
    renderApp('/products')
    expect(screen.getByText('Products Page')).toBeInTheDocument()
  })

  it('renders Users page on /users', () => {
    renderApp('/users')
    expect(screen.getByText('Users Page')).toBeInTheDocument()
  })

  it('renders Orders page on /orders', () => {
    renderApp('/orders')
    expect(screen.getByText('Orders Page')).toBeInTheDocument()
  })
})
