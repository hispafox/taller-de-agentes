import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { abrirIncidencia, type DatosDeIncidencia } from '../api';
import FormularioIncidencia from '../componentes/FormularioIncidencia';

const vacia: DatosDeIncidencia = {
  titulo: '',
  descripcion: null,
  observaciones: null,
  prioridad: 'Normal',
  zonaId: null,
};

export default function Nueva() {
  const navegar = useNavigate();
  const [error, setError] = useState<string | null>(null);

  function guardar(datos: DatosDeIncidencia) {
    abrirIncidencia(datos)
      .then((creada) => navegar(`/incidencias/${creada.id}`))
      .catch((e: Error) => setError(e.message));
  }

  return (
    <>
      <h1>Nueva incidencia</h1>
      {error && <p role="alert">{error}</p>}
      <FormularioIncidencia inicial={vacia} textoBoton="Abrir incidencia" alGuardar={guardar} />
    </>
  );
}
