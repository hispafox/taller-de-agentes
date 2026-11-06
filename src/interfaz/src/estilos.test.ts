import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { expect, it } from 'vitest';

it('el rojo de la aplicación es el que da contraste sobre blanco', () => {
  const estilos = readFileSync(resolve(__dirname, 'estilos.css'), 'utf8');

  expect(estilos).toContain('--rojo: #c62828;');
});
