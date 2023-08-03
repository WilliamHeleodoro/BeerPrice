using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DTO
{
    public class CervejaMaiorPrecoDTO
    {
        public string Mercado { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Marca { get; set; } = "";
        public string Caracteristica { get; set; } = "";
        public string Quantidade { get; set; } = "";
        public int Unidade { get; set; }
        public decimal Preco { get; set; }

    }
}
