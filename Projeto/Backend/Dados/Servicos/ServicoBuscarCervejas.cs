using Dados.DTO;
using Dados.Entidades;
using Dados.Filtros;
using Dados.Repositorios;
using WebScraping.Model;

namespace Dados.Servicos
{
    public class ServicoBuscarCervejas
    {
        private readonly RepositorioBuscarCervejas _repositorioBuscarCervejas;

        public ServicoBuscarCervejas (
            RepositorioBuscarCervejas repositorioBuscarCervejas
            ) 
        {
            _repositorioBuscarCervejas = repositorioBuscarCervejas;
        }
        public PaginacaoResponse<CervejaDTO> BuscarCervejas(PaginacaoDTO paginacao)
        {
            return _repositorioBuscarCervejas.BuscarCatalogoCervejas(paginacao);
        }
    }
}
