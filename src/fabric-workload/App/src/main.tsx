import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import { PublicClientApplication } from '@azure/msal-browser'
import { MsalProvider } from '@azure/msal-react'
import { msalConfig } from './auth/msalConfig'
import { setMsalInstance } from './api/client'
import App from './App'
import './App.css'

const msalInstance = new PublicClientApplication(msalConfig)
setMsalInstance(msalInstance)

msalInstance.initialize().then(() => {
  ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <MsalProvider instance={msalInstance}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </MsalProvider>
    </React.StrictMode>,
  )
})
