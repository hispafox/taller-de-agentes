import { readFile, mkdir } from 'node:fs/promises';
import { chromium } from 'playwright';

const direccion = process.env.URL_APP ?? 'http://localhost:5173';
const manual = 'docs/manual/manual-usuario.md';
const carpeta = 'docs/manual/capturas';

// La ronda: cómo se llega a cada pantalla y qué tiene que verse antes de disparar.
const ronda = {
  'lista-cerradas': async (pagina) => {
    await pagina.goto(`${direccion}/incidencias/cerradas`);
    await pagina.getByRole('heading', { name: 'Incidencias cerradas' }).waitFor();
  },
  'archivar-incidencia': async (pagina) => {
    await pagina.goto(`${direccion}/incidencias/cerradas`);
    await pagina.getByRole('link', { name: 'Fuga en el garaje' }).click();
    await pagina.getByRole('button', { name: 'Archivar' }).waitFor();
  },
};

const texto = await readFile(manual, 'utf8');
const huecos = [...texto.matchAll(/<!-- captura: ([a-z0-9-]+) -->/g)].map((m) => m[1]);

const sinPaso = huecos.filter((id) => !ronda[id]);
if (sinPaso.length > 0) {
  console.error(`Huecos sin paso en la ronda: ${sinPaso.join(', ')}`);
  process.exit(2);
}

await mkdir(carpeta, { recursive: true });
const navegador = await chromium.launch();
const contexto = await navegador.newContext({
  viewport: { width: 1280, height: 800 },
  locale: 'es-ES',
  timezoneId: 'Europe/Madrid',
});
const pagina = await contexto.newPage();
pagina.setDefaultTimeout(5000);
await pagina.clock.setFixedTime(new Date('2026-11-02T10:00:00'));

let fallos = 0;
for (const id of huecos) {
  try {
    await ronda[id](pagina);
    await pagina.screenshot({ path: `${carpeta}/${id}.png`, animations: 'disabled' });
    console.log(`ok     ${id}`);
  } catch (error) {
    fallos += 1;
    console.error(`FALLO  ${id}: ${error.message.split('\n')[0]}`);
  }
}

await navegador.close();
process.exit(fallos > 0 ? 1 : 0);
