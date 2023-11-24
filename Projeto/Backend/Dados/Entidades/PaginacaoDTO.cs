using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.Entidades
{
    public class PaginacaoDTO
    {
        public long Linhas { get; set; } = 10;
        public long Pagina { get; set; } = 1;
        public string? Pesquisa { get; set; } = "";
    }
}
