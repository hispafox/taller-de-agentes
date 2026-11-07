import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { expect, it } from 'vitest';

it('el rojo de la aplicación es el que da contraste sobre blanco', () => {
  const estilos = readFileSync(resolve(__dirname, 'estilos.css'), 'utf8');

  expect(estilos).toContain('--rojo: #c62828;');
});

it('las fechas de la tabla tienen ancho fijo, para que el recuadro del mask no cambie', () => {
  const estilos = readFileSync(resolve(__dirname, 'estilos.css'), 'utf8');

  expect(estilos).toMatch(/td time \{\s*display: inline-block;\s*width: 7em;\s*\}/);
});
