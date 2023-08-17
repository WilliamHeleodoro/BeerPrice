using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DTO
{
    public class CervejaHistoricoPrecoDTO
    {
        public string Mercado { get; set; } = "";
        public decimal Preco { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
