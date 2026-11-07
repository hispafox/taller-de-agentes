import { Link } from 'react-router-dom';
import type { IncidenciaEnLista } from '../api';

const fecha = new Intl.DateTimeFormat('es-ES', { dateStyle: 'short' });

function Plazo({ incidencia }: { incidencia: IncidenciaEnLista }) {
  if (!incidencia.plazo) {
    return null;
  }

  const dia = fecha.format(new Date(`${incidencia.plazo}T00:00:00`));
  const cuando = <time dateTime={incidencia.plazo}>{dia}</time>;
  return incidencia.fueraDePlazo ? <span className="aviso-rojo">{cuando} · Fuera de plazo</span> : cuando;
}

export default function TablaIncidencias({ incidencias }: { incidencias: IncidenciaEnLista[] }) {
  if (incidencias.length === 0) {
    return <p>No hay incidencias.</p>;
  }

  return (
    <table>
      <thead>
        <tr>
          <th scope="col">Título</th>
          <th scope="col">Prioridad</th>
          <th scope="col">Estado</th>
          <th scope="col">Zona</th>
          <th scope="col">Técnico</th>
          <th scope="col">Plazo</th>
          <th scope="col">Apertura</th>
        </tr>
      </thead>
      <tbody>
        {incidencias.map((incidencia) => (
          <tr key={incidencia.id}>
            <td>
              <Link to={`/incidencias/${incidencia.id}`}>{incidencia.titulo}</Link>
            </td>
            <td className={incidencia.prioridad === 'Urgente' ? 'aviso-rojo' : undefined}>
              {incidencia.prioridad}
            </td>
            <td>{incidencia.estado}</td>
            <td>{incidencia.zona ?? 'Sin zona'}</td>
            <td>{incidencia.tecnico ?? 'Sin asignar'}</td>
            <td>
              <Plazo incidencia={incidencia} />
            </td>
            <td>
              <time dateTime={incidencia.fechaApertura}>{fecha.format(new Date(incidencia.fechaApertura))}</time>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
