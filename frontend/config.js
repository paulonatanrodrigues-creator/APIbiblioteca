// config.js
// Endereço base da API .NET (APIbiblioteca).
// Altere aqui caso a API esteja rodando em outra porta/host.
const API_BASE_URL = "http://localhost:5114/api";

const API_ENDPOINTS = {
  autor: `${API_BASE_URL}/Autor`,
  categoria: `${API_BASE_URL}/Categoria`,
  emprestimo: `${API_BASE_URL}/Emprestimo`,
  livro: `${API_BASE_URL}/Livro`,
  usuario: `${API_BASE_URL}/Usuario`,
};
