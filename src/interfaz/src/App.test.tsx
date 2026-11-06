import { render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import { describe, expect, it, vi } from 'vitest';
import App from './App';
import type { Incidencia, IncidenciaEnLista } from './api';

const fuga: IncidenciaEnLista = {
  id: 1,
  titulo: 'Fuga en el garaje, plaza 14',
  prioridad: 'Urgente',
  estado: 'Abierta',
  zona: 'Norte',
  fechaApertura: '2026-11-02T10:00:00Z',
};

const fichaDe = (estado: Incidencia['estado']): Incidencia => ({
  ...fuga,
  estado,
  descripcion: null,
  observaciones: null,
  zonaId: 1,
  fechaResolucion: null,
  fechaCierre: null,
});

function responder(rutas: Record<string, unknown>) {
  const simulado = vi.fn(async (url: string, opciones?: RequestInit) => {
    const clave = `${opciones?.method ?? 'GET'} ${url}`;
    const encontrada = Object.keys(rutas).find((ruta) => clave.startsWith(ruta));
    return new Response(JSON.stringify(encontrada ? rutas[encontrada] : []), { status: 200 });
  });
  vi.stubGlobal('fetch', simulado);
  return simulado;
}

const abrir = (ruta: string) =>
  render(
    <MemoryRouter initialEntries={[ruta]}>
      <App />
    </MemoryRouter>,
  );

describe('la lista de incidencias', () => {
  it('enseña cada incidencia en su fila, con su enlace a la ficha', async () => {
    responder({ 'GET /api/incidencias': [fuga], 'GET /api/zonas': [] });
    abrir('/incidencias');

    const fila = await screen.findByRole('row', { name: /Fuga en el garaje/ });
    expect(within(fila).getByRole('link', { name: /Fuga en el garaje/ })).toHaveAttribute('href', '/incidencias/1');
  });

  it('deja fuera las cerradas, que tienen su pantalla', async () => {
    responder({
      'GET /api/incidencias': [fuga, { ...fuga, id: 2, titulo: 'Caldera con error E04', estado: 'Cerrada' }],
      'GET /api/zonas': [],
    });
    abrir('/incidencias');

    await screen.findByRole('row', { name: /Fuga en el garaje/ });
    expect(screen.queryByRole('row', { name: /Caldera/ })).not.toBeInTheDocument();
  });

  it('pide a la API el filtro elegido', async () => {
    const simulado = responder({ 'GET /api/incidencias': [fuga], 'GET /api/zonas': [] });
    abrir('/incidencias');

    await userEvent.selectOptions(await screen.findByLabelText('Estado'), 'Resuelta');

    expect(simulado).toHaveBeenCalledWith('/api/incidencias?estado=Resuelta', undefined);
  });

  it('marca la prioridad urgente con el texto y con la clase del rojo', async () => {
    responder({ 'GET /api/incidencias': [fuga], 'GET /api/zonas': [] });
    abrir('/incidencias');

    expect(await screen.findByText('Urgente', { selector: 'td' })).toHaveClass('aviso-rojo');
  });
});

describe('las cerradas', () => {
  it('tienen su título y solo piden las cerradas', async () => {
    const simulado = responder({ 'GET /api/incidencias': [] });
    abrir('/incidencias/cerradas');

    expect(await screen.findByRole('heading', { name: 'Incidencias cerradas' })).toBeInTheDocument();
    expect(simulado).toHaveBeenCalledWith('/api/incidencias?estado=Cerrada', undefined);
  });
});

describe('la ficha', () => {
  it('deja guardar y resolver una incidencia abierta', async () => {
    responder({ 'GET /api/incidencias/1': fichaDe('Abierta'), 'GET /api/zonas': [] });
    abrir('/incidencias/1');

    expect(await screen.findByRole('button', { name: 'Guardar' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Resolver' })).toBeInTheDocument();
    expect(screen.queryByRole('button', { name: 'Cerrar' })).not.toBeInTheDocument();
  });

  it('al guardar manda solo los campos que se editan', async () => {
    const simulado = responder({
      'PUT /api/incidencias/1': fichaDe('Abierta'),
      'GET /api/incidencias/1': fichaDe('Abierta'),
      'GET /api/zonas': [],
    });
    abrir('/incidencias/1');

    await userEvent.click(await screen.findByRole('button', { name: 'Guardar' }));

    const llamada = simulado.mock.calls.find(([, opciones]) => opciones?.method === 'PUT');
    expect(Object.keys(JSON.parse(llamada![1]!.body as string)).sort()).toEqual([
      'descripcion',
      'observaciones',
      'prioridad',
      'titulo',
      'zonaId',
    ]);
  });

  it('solo deja cerrar o reabrir una incidencia resuelta', async () => {
    responder({ 'GET /api/incidencias/1': fichaDe('Resuelta'), 'GET /api/zonas': [] });
    abrir('/incidencias/1');

    expect(await screen.findByRole('button', { name: 'Cerrar' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Reabrir' })).toBeInTheDocument();
  });

  it('no deja editar una incidencia cerrada', async () => {
    responder({ 'GET /api/incidencias/1': fichaDe('Cerrada'), 'GET /api/zonas': [] });
    abrir('/incidencias/1');

    expect(await screen.findByRole('button', { name: 'Reabrir' })).toBeInTheDocument();
    expect(screen.queryByRole('button', { name: 'Guardar' })).not.toBeInTheDocument();
    expect(screen.getByLabelText('Título')).toBeDisabled();
  });
});
