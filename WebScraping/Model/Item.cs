using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Model
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("mercado")]
        public string Mercado { get; set; } = "";

        [Column("marca")]
        public string Marca { get; set; } = "";

        [Column("titulo")]
        public string Titulo { get; set; } = "";

        [Column("preco")]
        public decimal Preco { get; set; }

        [Column("unidade")]
        public string Unidade { get; set; } = "";

        [Column("quantidade")]
        public int Quantidade { get; set; }

    }
}
