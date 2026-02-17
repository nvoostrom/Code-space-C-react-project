import { useEffect, useState } from 'react'
import { api } from '../api/client'
import type { ProductSummary } from '../types'

function Products() {
  const [products, setProducts] = useState<ProductSummary[]>([])
  const [filter, setFilter] = useState('')
  const [error, setError] = useState('')

  useEffect(() => {
    api.getProducts().then(setProducts).catch(e => setError(e.message))
  }, [])

  if (error) return <div className="error">Error: {error}</div>
  if (!products.length) return <div className="loading">Loading...</div>

  const categories = [...new Set(products.map(p => p.category))].sort()
  const filtered = filter ? products.filter(p => p.category === filter) : products

  return (
    <div>
      <h2>Products</h2>
      <div className="filter-bar">
        <label>Category: </label>
        <select value={filter} onChange={e => setFilter(e.target.value)}>
          <option value="">All</option>
          {categories.map(c => <option key={c} value={c}>{c}</option>)}
        </select>
        <span className="count">{filtered.length} products</span>
      </div>
      <table className="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Category</th>
            <th>Price</th>
          </tr>
        </thead>
        <tbody>
          {filtered.map(p => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.name}</td>
              <td>{p.category}</td>
              <td>${p.price.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default Products
