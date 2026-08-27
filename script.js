// script.js
// Consome a API APIbiblioteca (.NET) e renderiza o histórico do leitor
// pesquisado, no formato: Dados do Leitor + Histórico de Empréstimos.

const inputNome = document.getElementById("inputNome");
const btnBuscar = document.getElementById("btnBuscar");
const statusArea = document.getElementById("statusArea");

const dadosLeitorBox = document.getElementById("dadosLeitorBox");
const leitorNomeEl = document.getElementById("leitorNome");
const leitorEmailEl = document.getElementById("leitorEmail");
const leitorStatusEl = document.getElementById("leitorStatus");

const historicoBox = document.getElementById("historicoBox");
const tabelaBody = document.getElementById("tabelaHistoricoBody");
const semEmprestimos = document.getElementById("semEmprestimos");

// ---------------------------------------------------------
// Helpers
// ---------------------------------------------------------

function showStatus(message, type) {
  statusArea.hidden = false;
  statusArea.textContent = message;
  statusArea.className = "status-area";
  if (type) statusArea.classList.add(`is-${type}`);
}

function hideStatus() {
  statusArea.hidden = true;
  statusArea.textContent = "";
}

function hideResults() {
  dadosLeitorBox.hidden = true;
  historicoBox.hidden = true;
}

// Converte "2026-05-01" (DateOnly do .NET) para "01/05/2026"
function formatDate(isoDate) {
  if (!isoDate) return "—";
  const [ano, mes, dia] = isoDate.split("-");
  if (!ano || !mes || !dia) return isoDate;
  return `${dia}/${mes}/${ano}`;
}

function normalize(str) {
  return (str || "")
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "") // remove acentos
    .trim()
    .toLowerCase();
}

async function fetchJson(url) {
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error(`Falha ao consultar ${url} (HTTP ${response.status})`);
  }
  return response.json();
}

// Mapeia o status da conta do usuário para uma tag visual
function renderStatusConta(status) {
  const value = (status || "").trim();
  const label = value || "Desconhecido";
  let cls = "tag--neutral";

  const normalized = normalize(value);
  if (normalized === "ativo") cls = "tag--ok";
  else if (normalized === "inativo" || normalized === "suspenso") cls = "tag--danger";

  leitorStatusEl.textContent = `[ ${label} ]`;
  leitorStatusEl.className = `tag ${cls}`;
}

// Mapeia o status do empréstimo para uma tag visual
function statusEmprestimoTag(emprestimo) {
  let label = (emprestimo.statusEmprestimo || "").trim();

  // Se não vier status explícito da API, deduz pela data de devolução efetiva.
  if (!label) {
    label = emprestimo.dataDevolucaoEfetiva ? "Devolvido" : "Pendente";
  }

  const normalized = normalize(label);
  let cls = "tag--neutral";

  if (normalized === "devolvido") cls = "tag--ok";
  else if (normalized === "pendente") cls = "tag--pending";
  else if (normalized.includes("atras")) cls = "tag--danger";

  const span = document.createElement("span");
  span.className = `tag ${cls}`;
  span.textContent = `[${label}]`;
  return span;
}

// ---------------------------------------------------------
// Busca principal
// ---------------------------------------------------------

async function buscarHistorico() {
  const termoBusca = inputNome.value.trim();

  if (!termoBusca) {
    hideResults();
    showStatus("Digite o nome completo do leitor para buscar.", "empty");
    return;
  }

  hideResults();
  showStatus("Buscando leitor...", "loading");
  btnBuscar.disabled = true;

  try {
    // 1) Busca todos os usuários e localiza o leitor pelo nome.
    const usuarios = await fetchJson(API_ENDPOINTS.usuario);

    const alvo = normalize(termoBusca);
    let leitor = usuarios.find((u) => normalize(u.nome) === alvo);
    if (!leitor) {
      // fallback: correspondência parcial, caso o nome não seja exatamente igual
      leitor = usuarios.find((u) => normalize(u.nome).includes(alvo));
    }

    if (!leitor) {
      showStatus(`Nenhum leitor encontrado com o nome "${termoBusca}".`, "empty");
      return;
    }

    // 2) Busca empréstimos e livros em paralelo.
    const [emprestimos, livros] = await Promise.all([
      fetchJson(API_ENDPOINTS.emprestimo),
      fetchJson(API_ENDPOINTS.livro),
    ]);

    const livrosPorId = new Map(livros.map((l) => [l.livroId, l]));

    const emprestimosDoLeitor = emprestimos
      .filter((e) => e.usuarioId === leitor.usuarioId)
      .sort((a, b) => (a.dataEmprestimo < b.dataEmprestimo ? 1 : -1));

    renderLeitor(leitor);
    renderHistorico(emprestimosDoLeitor, livrosPorId);

    hideStatus();
    dadosLeitorBox.hidden = false;
    historicoBox.hidden = false;
  } catch (err) {
    console.error(err);
    showStatus(
      `Não foi possível conectar à API em ${API_BASE_URL}. Verifique se ela está em execução. (${err.message})`,
      "error"
    );
  } finally {
    btnBuscar.disabled = false;
  }
}

function renderLeitor(leitor) {
  leitorNomeEl.textContent = leitor.nome || "—";
  leitorEmailEl.textContent = leitor.email || "—";
  renderStatusConta(leitor.statusConta);
}

function renderHistorico(emprestimosDoLeitor, livrosPorId) {
  tabelaBody.innerHTML = "";

  if (emprestimosDoLeitor.length === 0) {
    semEmprestimos.hidden = false;
    return;
  }

  semEmprestimos.hidden = true;

  for (const emprestimo of emprestimosDoLeitor) {
    const livro = livrosPorId.get(emprestimo.livroId);
    const titulo = livro ? livro.titulo : `Livro #${emprestimo.livroId}`;

    const tr = document.createElement("tr");

    const tdLivro = document.createElement("td");
    tdLivro.textContent = titulo;

    const tdEmprestimo = document.createElement("td");
    tdEmprestimo.textContent = formatDate(emprestimo.dataEmprestimo);

    const tdPrazo = document.createElement("td");
    tdPrazo.textContent = formatDate(emprestimo.dataDevolucaoPrevista);

    const tdStatus = document.createElement("td");
    tdStatus.appendChild(statusEmprestimoTag(emprestimo));

    tr.append(tdLivro, tdEmprestimo, tdPrazo, tdStatus);
    tabelaBody.appendChild(tr);
  }
}

// ---------------------------------------------------------
// Eventos
// ---------------------------------------------------------

btnBuscar.addEventListener("click", buscarHistorico);

inputNome.addEventListener("keydown", (event) => {
  if (event.key === "Enter") {
    buscarHistorico();
  }
});
