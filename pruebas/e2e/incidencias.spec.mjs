import { test, expect } from '@playwright/test';

test('C-07 · una incidencia con plazo de ayer sale fuera de plazo', async ({ page }) => {
  await page.goto('/incidencias');

  const fila = page.getByRole('row', { name: /Fuga en el garaje/ });
  const aviso = fila.getByText('Fuera de plazo');
  await expect(aviso).toBeVisible();
  await expect(aviso).toHaveCSS('color', 'rgb(198, 40, 40)');
});

test('C-09 · el filtro deja solo las del técnico', async ({ page }) => {
  await page.goto('/incidencias');

  await page.getByLabel('Técnico').selectOption('Rafa');

  await expect(page.getByRole('row', { name: /Portero automático/ })).toHaveCount(0);
  await expect(page.getByRole('row', { name: /Fuga en el garaje/ })).toBeVisible();
});
