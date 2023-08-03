using Dados.DTO;
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
        public List<CervejaDTO> BuscarCervejas(FiltroObterCerveja filtroObterCerveja)
        {
            return _repositorioBuscarCervejas.BuscarCatalogoCervejas(filtroObterCerveja);
        }
    }
}
