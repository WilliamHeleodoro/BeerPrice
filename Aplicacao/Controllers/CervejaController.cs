using Dados.Filtros;
using Dados.Servicos;
using Microsoft.AspNetCore.Mvc;


namespace Dados.Controller
{
    [ApiController]
    [Route("cervejas")]
    public class CervejaController : ControllerBase
    {
        [HttpPost]
        public ActionResult BuscarListaCervejas([FromServices] ServicoBuscarCervejas servicoBuscarCervejas,
            [FromBody] FiltroObterCerveja filtroObterCerveja
            )
        {
            var resultado = servicoBuscarCervejas.BuscarCervejas(filtroObterCerveja);

            if (resultado == null || resultado.Count == 0)
            {
                return BadRequest("Produto não existe"); // Retorna HTTP 400 - Bad Request
            }

            return Ok(resultado);
        }
    }
}
