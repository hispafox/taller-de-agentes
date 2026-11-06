import { useEffect, useState } from 'react';
import { listarIncidencias, type IncidenciaEnLista } from '../api';
import TablaIncidencias from '../componentes/TablaIncidencias';

export default function Cerradas() {
  const [incidencias, setIncidencias] = useState<IncidenciaEnLista[]>([]);

  useEffect(() => {
    listarIncidencias({ estado: 'Cerrada' }).then(setIncidencias);
  }, []);

  return (
    <>
      <h1>Incidencias cerradas</h1>
      <TablaIncidencias incidencias={incidencias} />
    </>
  );
}
