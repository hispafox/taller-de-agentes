import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: 'pruebas/e2e',
  use: { baseURL: 'http://localhost:5173' },
  webServer: [
    {
      command: 'dotnet run --project src',
      url: 'https://localhost:5001/zonas',
      ignoreHTTPSErrors: true,
      reuseExistingServer: true,
    },
    {
      command: 'npm run dev --prefix src/interfaz',
      url: 'http://localhost:5173',
      reuseExistingServer: true,
    },
  ],
});
