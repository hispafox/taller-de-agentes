// Pasa un Markdown de docs/ a PDF con un navegador. Uso: node md-a-pdf.mjs <ruta>
// El navegador es el de Playwright. Con CANAL_NAVEGADOR=msedge usa el Edge instalado.
// Salidas: 0 bien · 1 el repositorio no está listo · 2 falta una herramienta.
import { execFileSync } from 'node:child_process';
import { mkdirSync, readFileSync } from 'node:fs';
import { basename, dirname, extname, resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

async function cargar(paquete) {
  try {
    return await import(paquete);
  } catch {
    console.log(`Falta el paquete ${paquete}. Instálalo con: npm install --save-dev marked playwright`);
    process.exit(2);
  }
}

const { chromium } = await cargar('playwright');
const { marked } = await cargar('marked');

const fuente = process.argv[2];
if (!fuente) {
  console.log('Uso: node md-a-pdf.mjs <ruta del documento>');
  process.exit(2);
}

const git = (...args) => execFileSync('git', args, { encoding: 'utf8', stdio: ['ignore', 'pipe', 'ignore'] }).trim();

if (git('status', '--porcelain', '--', 'docs')) {
  console.log('Hay cambios sin confirmar en docs/. Haz el commit antes de exportar.');
  process.exit(1);
}

let version;
try {
  version = git('describe', '--tags', '--exact-match');
} catch {
  console.log('El commit actual no tiene etiqueta de versión. Etiqueta lo que vas a entregar.');
  process.exit(1);
}

const commit = git('log', '-1', '--format=%h', '--', fuente);
const fecha = git('log', '-1', '--format=%cs', '--', fuente);
const proyecto = process.env.PROYECTO ?? 'GI';

const estilos = `
  body { font-family: 'Segoe UI', Arial, sans-serif; font-size: 11pt; line-height: 1.4; color: #1f2933; }
  table { width: 100%; border-collapse: collapse; margin: 1em 0; }
  th, td { border: 1px solid #b8c0c8; padding: 4px 8px; text-align: left; }
  img { max-width: 100%; }
  code { font-family: Consolas, monospace; }
`;
const control = `<table><tr><th>Proyecto</th><th>Versión</th><th>Fecha</th><th>Origen</th></tr>
  <tr><td>${proyecto}</td><td>${version}</td><td>${fecha}</td><td>${commit}</td></tr></table>`;
const base = pathToFileURL(`${resolve(dirname(fuente))}/`).href;
const html = `<!doctype html><html lang="es"><head><meta charset="utf-8"><base href="${base}"><style>${estilos}</style></head>
  <body>${control}${marked.parse(readFileSync(fuente, 'utf8'))}</body></html>`;

let navegador;
try {
  navegador = await chromium.launch(process.env.CANAL_NAVEGADOR ? { channel: process.env.CANAL_NAVEGADOR } : {});
} catch {
  console.log('No encuentro el navegador. Instálalo con: npx playwright install chromium');
  process.exit(2);
}

mkdirSync('salida', { recursive: true });
const destino = `salida/${basename(fuente, extname(fuente))}-${version}.pdf`;
const pagina = await navegador.newPage();
await pagina.setContent(html, { waitUntil: 'load' });
await pagina.pdf({ path: destino, format: 'A4', margin: { top: '2cm', bottom: '2cm', left: '2cm', right: '2cm' } });
await navegador.close();
console.log(`Exportado: ${destino} (${version}, ${commit})`);
