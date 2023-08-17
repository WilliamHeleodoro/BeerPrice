import api from "./api";

export function getCervejaDetalhes(id) {
  return api.get(`/cervejas/${id}`);
}

export function getHistoricoPreco(id, filtroMercado) {
  return api.get(`/cervejas/historico/${id}?filtroMercado=${filtroMercado}`);
}