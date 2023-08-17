using Dados.DTO;
using Dados.Filtros;
using Dados.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.Servicos
{
    public class ServicoBuscarHistoricoPrecos
    {
        private readonly RepositorioBuscarHistoricoPrecos _repositorioBuscarHistoricoPrecos;

        public ServicoBuscarHistoricoPrecos(
           RepositorioBuscarHistoricoPrecos repositorioBuscarHistoricoPrecos
           )
        {
            _repositorioBuscarHistoricoPrecos = repositorioBuscarHistoricoPrecos;
        }
        public List<CervejaHistoricoPrecoDTO> BuscarCervejaPreco(long codigo, FiltroObterHistorico filtros)
        {
            return _repositorioBuscarHistoricoPrecos.BuscarHistoricoPreco(codigo, filtros);
        }
    }

}
