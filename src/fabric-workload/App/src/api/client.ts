export async function fetchJson<T>(path: string): Promise<T> {
  const response = await fetch(path);
  if (!response.ok) {
    throw new Error(`API error: ${response.status}`);
  }
  return response.json();
}

export const api = {
  getProducts: () => fetchJson<import('../types').ProductSummary[]>('/api/content/products'),
  getUsers: () => fetchJson<import('../types').UserSummary[]>('/api/content/users'),
  getOrders: () => fetchJson<import('../types').OrderSummary[]>('/api/content/orders'),
  getDashboard: () => fetchJson<import('../types').DashboardData>('/api/content/dashboard'),
};
