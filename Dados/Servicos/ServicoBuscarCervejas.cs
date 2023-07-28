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
        public List<Item> BuscarCervejas()
        {
            return _repositorioBuscarCervejas.BuscarCatalogoCervejas();
        }
    }
}
