import { Link, Navigate, Route, Routes } from 'react-router-dom';
import Cerradas from './paginas/Cerradas';
import Ficha from './paginas/Ficha';
import Lista from './paginas/Lista';
import Nueva from './paginas/Nueva';

export default function App() {
  return (
    <>
      <nav aria-label="Principal">
        <Link to="/incidencias">Incidencias</Link>
        <Link to="/incidencias/cerradas">Cerradas</Link>
        <Link to="/incidencias/nueva">Nueva incidencia</Link>
      </nav>
      <main>
        <Routes>
          <Route path="/" element={<Navigate to="/incidencias" replace />} />
          <Route path="/incidencias" element={<Lista />} />
          <Route path="/incidencias/cerradas" element={<Cerradas />} />
          <Route path="/incidencias/nueva" element={<Nueva />} />
          <Route path="/incidencias/:id" element={<Ficha />} />
        </Routes>
      </main>
    </>
  );
}
