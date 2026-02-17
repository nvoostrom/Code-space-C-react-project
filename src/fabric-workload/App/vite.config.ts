import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: parseInt(process.env.PORT || '5173'),
    proxy: {
      '/api': {
        target: process.env.services__content_api__https__0
             || process.env.services__content_api__http__0
             || 'http://localhost:5200',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
