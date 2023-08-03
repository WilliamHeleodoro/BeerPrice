using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DTO
{
    public class CervejaPorCodigoDTO
    {
        public string Titulo { get; set; } = "";
        public string Imagem { get; set; } = "";
        public List<CervejaMaiorPrecoDTO> MaiorPreco { get; set; } = new();
    }
}
