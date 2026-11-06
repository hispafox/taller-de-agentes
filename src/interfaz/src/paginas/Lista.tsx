import { useEffect, useState, type FormEvent } from 'react';
import {
  buscarIncidencias,
  listarIncidencias,
  listarZonas,
  type Filtros,
  type IncidenciaEnLista,
  type Zona,
} from '../api';
import TablaIncidencias from '../componentes/TablaIncidencias';

export default function Lista() {
  const [incidencias, setIncidencias] = useState<IncidenciaEnLista[]>([]);
  const [zonas, setZonas] = useState<Zona[]>([]);
  const [filtros, setFiltros] = useState<Filtros>({});
  const [texto, setTexto] = useState('');

  useEffect(() => {
    listarZonas().then(setZonas);
  }, []);

  useEffect(() => {
    listarIncidencias(filtros).then(setIncidencias);
  }, [filtros]);

  function buscar(evento: FormEvent) {
    evento.preventDefault();
    const peticion = texto ? buscarIncidencias(texto) : listarIncidencias(filtros);
    peticion.then(setIncidencias);
  }

  // Las cerradas tienen su pantalla: aquí solo salen si se piden con el filtro o la búsqueda.
  const visibles = filtros.estado || texto ? incidencias : incidencias.filter((i) => i.estado !== 'Cerrada');

  return (
    <>
      <h1>Incidencias</h1>
      <div className="filtros">
        <label>
          Estado
          <select onChange={(e) => setFiltros({ ...filtros, estado: e.target.value })}>
            <option value="">En curso</option>
            <option>Abierta</option>
            <option>Resuelta</option>
            <option>Cerrada</option>
          </select>
        </label>
        <label>
          Prioridad
          <select onChange={(e) => setFiltros({ ...filtros, prioridad: e.target.value })}>
            <option value="">Todas</option>
            <option>Normal</option>
            <option>Alta</option>
            <option>Urgente</option>
          </select>
        </label>
        <label>
          Zona
          <select onChange={(e) => setFiltros({ ...filtros, zonaId: e.target.value })}>
            <option value="">Todas</option>
            {zonas.map((zona) => (
              <option key={zona.id} value={zona.id}>
                {zona.nombre}
              </option>
            ))}
          </select>
        </label>
        <form onSubmit={buscar}>
          <label>
            Buscar
            <input type="search" value={texto} onChange={(e) => setTexto(e.target.value)} />
          </label>
          <button type="submit">Buscar</button>
        </form>
      </div>
      <TablaIncidencias incidencias={visibles} />
    </>
  );
}
