import { useEffect, useState } from 'react'
import { api } from '../api/client'
import type { OrderSummary } from '../types'

function Orders() {
  const [orders, setOrders] = useState<OrderSummary[]>([])
  const [error, setError] = useState('')

  useEffect(() => {
    api.getOrders().then(setOrders).catch(e => setError(e.message))
  }, [])

  if (error) return <div className="error">Error: {error}</div>
  if (!orders.length) return <div className="loading">Loading...</div>

  const statusClass = (status: string) => {
    switch (status) {
      case 'Delivered': return 'badge-delivered'
      case 'Shipped': return 'badge-shipped'
      case 'Processing': return 'badge-processing'
      case 'Pending': return 'badge-pending'
      case 'Cancelled': return 'badge-cancelled'
      default: return ''
    }
  }

  return (
    <div>
      <h2>Orders</h2>
      <table className="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>User ID</th>
            <th>Total</th>
            <th>Status</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          {orders.map(o => (
            <tr key={o.id}>
              <td>{o.id}</td>
              <td>{o.userId}</td>
              <td>${o.totalAmount.toFixed(2)}</td>
              <td><span className={`badge ${statusClass(o.status)}`}>{o.status}</span></td>
              <td>{new Date(o.date).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default Orders
