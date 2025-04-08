using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        public int Estoque { get; set; }

        public string? Imagem { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
