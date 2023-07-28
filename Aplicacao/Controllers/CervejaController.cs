using Dados.Servicos;
using Microsoft.AspNetCore.Mvc;


namespace Dados.Controller
{
    [ApiController]
    [Route("cervejas")]
    public class CervejaController : ControllerBase
    {
        [HttpGet]
        public ActionResult BuscarListaCervejas([FromServices] ServicoBuscarCervejas servicoBuscarCervejas)
        {
            var resultado = servicoBuscarCervejas.BuscarCervejas();

            if (resultado == null || resultado.Count == 0)
            {
                return BadRequest(); // Retorna HTTP 400 - Bad Request
            }

            return Ok(resultado);
        }
    }
}
