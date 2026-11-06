export type Estado = 'Abierta' | 'Resuelta' | 'Cerrada';
export type Prioridad = 'Normal' | 'Alta' | 'Urgente';

export interface IncidenciaEnLista {
  id: number;
  titulo: string;
  prioridad: Prioridad;
  estado: Estado;
  zona: string | null;
  fechaApertura: string;
  tecnico: string | null;
  plazo: string | null;
  fueraDePlazo: boolean;
}

export interface Incidencia extends Omit<IncidenciaEnLista, 'tecnico' | 'fueraDePlazo'> {
  tecnico: Tecnico | null;
  descripcion: string | null;
  observaciones: string | null;
  zonaId: number | null;
  fechaResolucion: string | null;
  fechaCierre: string | null;
  archivada: boolean;
}

export interface DatosDeIncidencia {
  titulo: string;
  descripcion: string | null;
  observaciones: string | null;
  prioridad: Prioridad;
  zonaId: number | null;
}

export interface Tecnico {
  id: number;
  nombre: string;
}

export interface Zona {
  id: number;
  nombre: string;
}

export interface Filtros {
  estado?: string;
  prioridad?: string;
  zonaId?: string;
  tecnicoId?: string;
}

async function pedir<T>(ruta: string, opciones?: RequestInit): Promise<T> {
  const respuesta = await fetch(`/api${ruta}`, opciones);
  if (!respuesta.ok) {
    const problema = await respuesta.json().catch(() => null);
    throw new Error(problema?.detail ?? 'No se ha podido completar la petición.');
  }
  return respuesta.json();
}

function conCuerpo(method: string, cuerpo: unknown): RequestInit {
  return {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(cuerpo),
  };
}

export function listarIncidencias(filtros: Filtros = {}) {
  const consulta = new URLSearchParams(
    Object.entries(filtros).filter(([, valor]) => valor),
  ).toString();
  return pedir<IncidenciaEnLista[]>(`/incidencias${consulta ? `?${consulta}` : ''}`);
}

export function buscarIncidencias(texto: string) {
  return pedir<IncidenciaEnLista[]>(`/incidencias/buscar?texto=${encodeURIComponent(texto)}`);
}

export function leerIncidencia(id: string) {
  return pedir<Incidencia>(`/incidencias/${id}`);
}

export function abrirIncidencia(datos: DatosDeIncidencia) {
  return pedir<Incidencia>('/incidencias', conCuerpo('POST', datos));
}

export function editarIncidencia(id: string, datos: DatosDeIncidencia) {
  return pedir<Incidencia>(`/incidencias/${id}`, conCuerpo('PUT', datos));
}

export function cambiarEstado(id: string, accion: 'resolver' | 'cerrar' | 'reabrir' | 'archivar') {
  return pedir<Incidencia>(`/incidencias/${id}/${accion}`, { method: 'POST' });
}

export function asignarIncidencia(id: string, tecnicoId: number | null, plazo: string | null) {
  return pedir<unknown>(`/incidencias/${id}/asignacion`, conCuerpo('PUT', { tecnicoId, plazo }));
}

export function listarTecnicos() {
  return pedir<Tecnico[]>('/tecnicos');
}

export function listarZonas() {
  return pedir<Zona[]>('/zonas');
}
