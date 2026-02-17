import { Routes, Route, NavLink } from 'react-router-dom'
import { useIsAuthenticated, useMsal } from '@azure/msal-react'
import { loginRequest } from './auth/msalConfig'
import Dashboard from './pages/Dashboard'
import Products from './pages/Products'
import Users from './pages/Users'
import Orders from './pages/Orders'

function App() {
  const isAuthenticated = useIsAuthenticated()
  const { instance, accounts } = useMsal()

  const handleLogin = () => {
    instance.loginPopup(loginRequest)
  }

  const handleLogout = () => {
    instance.logoutPopup()
  }

  if (!isAuthenticated) {
    return (
      <div className="app">
        <nav className="navbar">
          <h1>Fabric Workload</h1>
        </nav>
        <main className="content">
          <div className="login-prompt">
            <h2>Welcome</h2>
            <p>Please sign in with your Microsoft account to continue.</p>
            <button className="login-button" onClick={handleLogin}>
              Sign In with Microsoft
            </button>
          </div>
        </main>
      </div>
    )
  }

  return (
    <div className="app">
      <nav className="navbar">
        <h1>Fabric Workload</h1>
        <div className="nav-links">
          <NavLink to="/">Dashboard</NavLink>
          <NavLink to="/products">Products</NavLink>
          <NavLink to="/users">Users</NavLink>
          <NavLink to="/orders">Orders</NavLink>
        </div>
        <div className="user-info">
          <span className="user-name">{accounts[0]?.name || accounts[0]?.username}</span>
          <button className="logout-button" onClick={handleLogout}>Sign Out</button>
        </div>
      </nav>
      <main className="content">
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/products" element={<Products />} />
          <Route path="/users" element={<Users />} />
          <Route path="/orders" element={<Orders />} />
        </Routes>
      </main>
    </div>
  )
}

export default App
