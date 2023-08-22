using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DTO
{
    public class CervejaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string Marca { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Caracteristica { get; set; } = "";
        public string Quantidade { get; set; } = "";
        public int Unidade { get; set; }
        public string Imagem { get; set; } = "";
        public DateTime DataAtualizacao { get; set; }
    }
}
