import api from "./api";

export function getCervejasPaginado(paginacao) {
  return api.get(`/cervejas?pagina=${paginacao.pagina}&linhas=${paginacao.linhas}&pesquisa=${paginacao.pesquisa}`);
}

export function getCervejaDetalhes(id) {
  return api.get(`/cervejas/${id}`);
}

export function getHistoricoPreco(id, filtroMercado) {
  return api.get(`/cervejas/historico/${id}?filtroMercado=${filtroMercado}`);
}
