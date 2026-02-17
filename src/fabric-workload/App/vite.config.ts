/// <reference types="vitest" />
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './src/test/setup.ts',
    exclude: ['e2e/**', 'node_modules/**'],
  },
  server: {
    port: parseInt(process.env.PORT || '5173'),
    hmr: process.env.CODESPACE_NAME
      ? {
          host: `${process.env.CODESPACE_NAME}-5173.${process.env.GITHUB_CODESPACES_PORT_FORWARDING_DOMAIN}`,
          protocol: 'wss',
          clientPort: 443,
        }
      : undefined,
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
