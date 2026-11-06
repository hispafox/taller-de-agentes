import { useEffect, useState, type FormEvent } from 'react';
import { listarZonas, type DatosDeIncidencia, type Prioridad, type Zona } from '../api';

interface Propiedades {
  inicial: DatosDeIncidencia;
  soloLectura?: boolean;
  textoBoton: string;
  alGuardar: (datos: DatosDeIncidencia) => void;
}

export default function FormularioIncidencia({ inicial, soloLectura, textoBoton, alGuardar }: Propiedades) {
  const [datos, setDatos] = useState(inicial);
  const [zonas, setZonas] = useState<Zona[]>([]);

  useEffect(() => {
    listarZonas().then(setZonas);
  }, []);

  function enviar(evento: FormEvent) {
    evento.preventDefault();
    alGuardar(datos);
  }

  return (
    <form onSubmit={enviar}>
      <fieldset disabled={soloLectura}>
        <label>
          Título
          <input
            required
            value={datos.titulo}
            onChange={(e) => setDatos({ ...datos, titulo: e.target.value })}
          />
        </label>
        <label>
          Descripción
          <textarea
            value={datos.descripcion ?? ''}
            onChange={(e) => setDatos({ ...datos, descripcion: e.target.value || null })}
          />
        </label>
        <label>
          Observaciones
          <textarea
            value={datos.observaciones ?? ''}
            onChange={(e) => setDatos({ ...datos, observaciones: e.target.value || null })}
          />
        </label>
        <label>
          Prioridad
          <select
            value={datos.prioridad}
            onChange={(e) => setDatos({ ...datos, prioridad: e.target.value as Prioridad })}
          >
            <option>Normal</option>
            <option>Alta</option>
            <option>Urgente</option>
          </select>
        </label>
        <label>
          Zona
          <select
            value={datos.zonaId ?? ''}
            onChange={(e) => setDatos({ ...datos, zonaId: e.target.value ? Number(e.target.value) : null })}
          >
            <option value="">Sin zona</option>
            {zonas.map((zona) => (
              <option key={zona.id} value={zona.id}>
                {zona.nombre}
              </option>
            ))}
          </select>
        </label>
      </fieldset>
      {!soloLectura && <button type="submit">{textoBoton}</button>}
    </form>
  );
}
