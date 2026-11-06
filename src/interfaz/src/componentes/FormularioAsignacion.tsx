import { useEffect, useState, type FormEvent } from 'react';
import { listarTecnicos, type Incidencia, type Tecnico } from '../api';

interface Propiedades {
  incidencia: Incidencia;
  alGuardar: (tecnicoId: number | null, plazo: string | null) => void;
}

export default function FormularioAsignacion({ incidencia, alGuardar }: Propiedades) {
  const [tecnicos, setTecnicos] = useState<Tecnico[]>([]);
  const [tecnicoId, setTecnicoId] = useState(incidencia.tecnico?.id ?? null);
  const [plazo, setPlazo] = useState(incidencia.plazo);

  useEffect(() => {
    listarTecnicos().then(setTecnicos);
  }, []);

  function enviar(evento: FormEvent) {
    evento.preventDefault();
    alGuardar(tecnicoId, plazo);
  }

  return (
    <form onSubmit={enviar}>
      <h2>Asignación</h2>
      <label>
        Técnico responsable
        <select value={tecnicoId ?? ''} onChange={(e) => setTecnicoId(e.target.value ? Number(e.target.value) : null)}>
          <option value="">Sin asignar</option>
          {tecnicos.map((tecnico) => (
            <option key={tecnico.id} value={tecnico.id}>
              {tecnico.nombre}
            </option>
          ))}
        </select>
      </label>
      <label>
        Plazo
        <input type="date" value={plazo ?? ''} onChange={(e) => setPlazo(e.target.value || null)} />
      </label>
      <button type="submit">Asignar</button>
    </form>
  );
}
