import { PublicClientApplication } from '@azure/msal-browser';
import { loginRequest } from '../auth/msalConfig';

let msalInstance: PublicClientApplication | null = null;

export function setMsalInstance(instance: PublicClientApplication) {
  msalInstance = instance;
}

async function getAccessToken(): Promise<string | null> {
  if (!msalInstance) return null;

  const accounts = msalInstance.getAllAccounts();
  if (accounts.length === 0) return null;

  try {
    const response = await msalInstance.acquireTokenSilent({
      ...loginRequest,
      account: accounts[0],
    });
    return response.accessToken;
  } catch {
    return null;
  }
}

export async function fetchJson<T>(path: string): Promise<T> {
  const headers: Record<string, string> = {};

  const token = await getAccessToken();
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  const response = await fetch(path, { headers });
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
