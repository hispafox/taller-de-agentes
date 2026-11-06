import { useCallback, useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { cambiarEstado, editarIncidencia, leerIncidencia, type DatosDeIncidencia, type Incidencia } from '../api';
import FormularioIncidencia from '../componentes/FormularioIncidencia';

export default function Ficha() {
  const { id = '' } = useParams();
  const [incidencia, setIncidencia] = useState<Incidencia | null>(null);
  const [error, setError] = useState<string | null>(null);

  const cargar = useCallback(() => {
    leerIncidencia(id).then(setIncidencia).catch((e: Error) => setError(e.message));
  }, [id]);

  useEffect(cargar, [cargar]);

  function guardar(datos: DatosDeIncidencia) {
    setError(null);
    editarIncidencia(id, datos).then(setIncidencia).catch((e: Error) => setError(e.message));
  }

  function accion(nombre: 'resolver' | 'cerrar' | 'reabrir' | 'archivar') {
    setError(null);
    cambiarEstado(id, nombre).then(setIncidencia).catch((e: Error) => setError(e.message));
  }

  if (!incidencia) {
    return error ? <p role="alert">{error}</p> : <p>Cargando…</p>;
  }

  const cerrada = incidencia.estado === 'Cerrada';

  return (
    <>
      <h1>{incidencia.titulo}</h1>
      <p>
        Estado: {incidencia.estado}
        {incidencia.archivada && ' (archivada)'}
      </p>
      {error && <p role="alert">{error}</p>}
      <FormularioIncidencia
        key={`${incidencia.id}-${incidencia.estado}`}
        inicial={{
          titulo: incidencia.titulo,
          descripcion: incidencia.descripcion,
          observaciones: incidencia.observaciones,
          prioridad: incidencia.prioridad,
          zonaId: incidencia.zonaId,
        }}
        soloLectura={cerrada}
        textoBoton="Guardar"
        alGuardar={guardar}
      />
      <div className="acciones">
        {incidencia.estado === 'Abierta' && <button onClick={() => accion('resolver')}>Resolver</button>}
        {incidencia.estado === 'Resuelta' && <button onClick={() => accion('cerrar')}>Cerrar</button>}
        {incidencia.estado !== 'Abierta' && <button onClick={() => accion('reabrir')}>Reabrir</button>}
        {cerrada && !incidencia.archivada && <button onClick={() => accion('archivar')}>Archivar</button>}
      </div>
    </>
  );
}
