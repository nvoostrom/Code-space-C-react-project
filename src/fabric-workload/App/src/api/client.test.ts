import { fetchJson, api } from './client'

beforeEach(() => {
  vi.restoreAllMocks()
})

describe('fetchJson', () => {
  it('returns parsed JSON on success', async () => {
    const mockData = { id: 1, name: 'Test' }
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: () => Promise.resolve(mockData),
    } as Response)

    const result = await fetchJson('/api/test')
    expect(result).toEqual(mockData)
    expect(fetch).toHaveBeenCalledWith('/api/test')
  })

  it('throws on non-ok response', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: false,
      status: 404,
    } as Response)

    await expect(fetchJson('/api/missing')).rejects.toThrow('API error: 404')
  })
})

describe('api', () => {
  beforeEach(() => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue({
      ok: true,
      json: () => Promise.resolve([]),
    } as Response)
  })

  it('getProducts calls /api/content/products', async () => {
    await api.getProducts()
    expect(fetch).toHaveBeenCalledWith('/api/content/products')
  })

  it('getUsers calls /api/content/users', async () => {
    await api.getUsers()
    expect(fetch).toHaveBeenCalledWith('/api/content/users')
  })

  it('getOrders calls /api/content/orders', async () => {
    await api.getOrders()
    expect(fetch).toHaveBeenCalledWith('/api/content/orders')
  })

  it('getDashboard calls /api/content/dashboard', async () => {
    await api.getDashboard()
    expect(fetch).toHaveBeenCalledWith('/api/content/dashboard')
  })
})
