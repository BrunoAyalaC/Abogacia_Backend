import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'https://localhost:7051', // Puerto correcto de LegalPro.Api
        changeOrigin: true,
        secure: false, // Ignorar certificados de desarrollo inválidos
      }
    }
  }
})
