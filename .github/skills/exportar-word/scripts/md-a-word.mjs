// Pasa un Markdown de docs/ a Word cuando no hay Pandoc. Uso: node md-a-word.mjs <ruta>
// Salidas: 0 bien · 1 el repositorio no está listo · 2 falta una herramienta.
import { execFileSync } from 'node:child_process';
import { mkdirSync, readFileSync, writeFileSync } from 'node:fs';
import { basename, extname } from 'node:path';

async function cargar(paquete) {
  try {
    return await import(paquete);
  } catch {
    console.log(`Falta el paquete ${paquete}. Instálalo con: npm install --save-dev docx marked`);
    process.exit(2);
  }
}

const { Document, HeadingLevel, Packer, Paragraph, Table, TableCell, TableRow, TextRun, WidthType } = await cargar('docx');
const { marked } = await cargar('marked');

const fuente = process.argv[2];
if (!fuente) {
  console.log('Uso: node md-a-word.mjs <ruta del documento>');
  process.exit(2);
}

const git = (...args) => execFileSync('git', args, { encoding: 'utf8', stdio: ['ignore', 'pipe', 'ignore'] }).trim();

if (git('status', '--porcelain', '--', 'docs')) {
  console.log('Hay cambios sin confirmar en docs/. Haz el commit antes de exportar.');
  process.exit(1);
}

let version;
try {
  version = git('describe', '--tags', '--abbrev=0');
} catch {
  console.log('El repositorio no tiene ninguna etiqueta de versión.');
  process.exit(1);
}

const commit = git('log', '-1', '--format=%h', '--', fuente);
const fecha = git('log', '-1', '--format=%cs', '--', fuente);
const proyecto = process.env.PROYECTO ?? 'GI';

const niveles = [HeadingLevel.HEADING_1, HeadingLevel.HEADING_2, HeadingLevel.HEADING_3, HeadingLevel.HEADING_4];

// De los trozos de una línea de Markdown a los trozos de texto de Word.
function trozos(tokens, formato = {}) {
  return (tokens ?? []).flatMap((token) => {
    if (token.type === 'strong') return trozos(token.tokens, { ...formato, bold: true });
    if (token.type === 'em') return trozos(token.tokens, { ...formato, italics: true });
    if (token.type === 'codespan') return [new TextRun({ ...formato, text: token.text, font: 'Consolas' })];
    if (token.tokens) return trozos(token.tokens, formato);
    return [new TextRun({ ...formato, text: token.text ?? '' })];
  });
}

function celda(contenido) {
  return new TableCell({ children: [new Paragraph({ children: trozos(contenido.tokens) })] });
}

function tabla(cabecera, filas) {
  return new Table({
    width: { size: 100, type: WidthType.PERCENTAGE },
    rows: [new TableRow({ tableHeader: true, children: cabecera.map(celda) }), ...filas.map((fila) => new TableRow({ children: fila.map(celda) }))],
  });
}

function bloques(token) {
  switch (token.type) {
    case 'heading':
      return [new Paragraph({ heading: niveles[Math.min(token.depth, 4) - 1], children: trozos(token.tokens) })];
    case 'paragraph':
      return [new Paragraph({ children: trozos(token.tokens) })];
    case 'list':
      return token.items.map((item) => new Paragraph({ bullet: { level: 0 }, children: trozos(item.tokens.flatMap((t) => t.tokens ?? [t])) }));
    case 'table':
      return [tabla(token.header, token.rows), new Paragraph({})];
    case 'code':
      return token.text.split('\n').map((linea) => new Paragraph({ children: [new TextRun({ text: linea, font: 'Consolas' })] }));
    default:
      return [];
  }
}

const control = tabla(
  ['Proyecto', 'Versión', 'Fecha', 'Origen'].map((texto) => ({ tokens: [{ type: 'text', text: texto }] })),
  [[proyecto, version, fecha, commit].map((texto) => ({ tokens: [{ type: 'text', text: texto }] }))],
);

const documento = new Document({
  sections: [{ children: [control, new Paragraph({}), ...marked.lexer(readFileSync(fuente, 'utf8')).flatMap(bloques)] }],
});

mkdirSync('salida', { recursive: true });
const destino = `salida/${basename(fuente, extname(fuente))}-${version}.docx`;
writeFileSync(destino, await Packer.toBuffer(documento));
console.log(`Exportado: ${destino} (${version}, ${commit})`);
