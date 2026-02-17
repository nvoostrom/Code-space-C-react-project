import { useEffect, useState } from 'react'
import { api } from '../api/client'
import type { UserSummary } from '../types'

function Users() {
  const [users, setUsers] = useState<UserSummary[]>([])
  const [error, setError] = useState('')

  useEffect(() => {
    api.getUsers().then(setUsers).catch(e => setError(e.message))
  }, [])

  if (error) return <div className="error">Error: {error}</div>
  if (!users.length) return <div className="loading">Loading...</div>

  return (
    <div>
      <h2>Users</h2>
      <table className="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
          </tr>
        </thead>
        <tbody>
          {users.map(u => (
            <tr key={u.id}>
              <td>{u.id}</td>
              <td>{u.name}</td>
              <td>{u.email}</td>
              <td><span className={`badge badge-${u.role.toLowerCase()}`}>{u.role}</span></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default Users
