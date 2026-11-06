/// <reference types="vitest/config" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    // La API y las pantallas comparten rutas (/incidencias), así que la interfaz llama a /api
    // y el proxy quita ese prefijo. Va por HTTPS con el certificado de desarrollo de .NET.
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        secure: false,
        rewrite: (ruta) => ruta.replace(/^\/api/, ''),
      },
    },
  },
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: './src/preparar-pruebas.ts',
  },
});
