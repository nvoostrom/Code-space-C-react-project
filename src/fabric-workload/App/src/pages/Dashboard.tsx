import { useEffect, useState } from 'react'
import { api } from '../api/client'
import type { DashboardData } from '../types'

function Dashboard() {
  const [data, setData] = useState<DashboardData | null>(null)
  const [error, setError] = useState('')

  useEffect(() => {
    api.getDashboard().then(setData).catch(e => setError(e.message))
  }, [])

  if (error) return <div className="error">Error: {error}</div>
  if (!data) return <div className="loading">Loading...</div>

  return (
    <div>
      <h2>Dashboard</h2>
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-value">{data.productCount}</div>
          <div className="stat-label">Products</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{data.userCount}</div>
          <div className="stat-label">Users</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{data.orderCount}</div>
          <div className="stat-label">Orders</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">${data.totalRevenue.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</div>
          <div className="stat-label">Total Revenue</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{data.pendingOrders}</div>
          <div className="stat-label">Pending Orders</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{data.deliveredOrders}</div>
          <div className="stat-label">Delivered Orders</div>
        </div>
        <div className="stat-card">
          <div className="stat-value">{data.cancelledOrders}</div>
          <div className="stat-label">Cancelled Orders</div>
        </div>
      </div>
    </div>
  )
}

export default Dashboard
