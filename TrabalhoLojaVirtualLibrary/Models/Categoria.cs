using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O Campo Descrção é obrigatório!")]
        public string? Descricao { get; set; }

        [JsonIgnore]
        public IEnumerable<Produto>? Produtos { get; set; }
    }
}
