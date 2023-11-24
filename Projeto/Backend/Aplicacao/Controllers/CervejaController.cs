using Dados.Entidades;
using Dados.Filtros;
using Dados.Servicos;
using Microsoft.AspNetCore.Mvc;


namespace Dados.Controller
{
    [ApiController]
    [Route("cervejas")]
    public class CervejaController : ControllerBase
    {
        [HttpGet]
        public ActionResult BuscarListaCervejasPaginado([FromServices] ServicoBuscarCervejas servicoBuscarCervejas,
            [FromQuery] PaginacaoDTO paginacao
            )
        {
            var resultado = servicoBuscarCervejas.BuscarCervejas(paginacao);

            return Ok(resultado);
        }

        [HttpGet("{codigo:long}")]
        public ActionResult BuscarCervejasMaiorPreco(long codigo, 
            [FromServices] ServicoBuscarCevejaPeloMaiorPreco servicoBuscarCevejaPeloMaiorPreco)
        {
            var resultado = servicoBuscarCevejaPeloMaiorPreco.BucarCervejaMaiorPreco(codigo);

            if (resultado.Titulo == "")
            {
                return BadRequest("Item não encontrado!"); // Retorna HTTP 400 - Bad Request
            }

            return Ok(resultado);
        }

        [HttpGet("historico/{codigo:long}")]
        public ActionResult BuscarHistoricoPrecos(long codigo,
            [FromServices] ServicoBuscarHistoricoPrecos servicoBuscarHistoricoPrecos,
            [FromQuery] FiltroObterHistorico filtros)
            
        {
            var resultado = servicoBuscarHistoricoPrecos.BuscarCervejaPreco(codigo, filtros);

            if (resultado == null)
            {
                return BadRequest("Item não encontrado!"); // Retorna HTTP 400 - Bad Request
            }

            return Ok(resultado);
        }
    }
}
