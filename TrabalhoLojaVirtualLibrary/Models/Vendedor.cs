using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class Vendedor
    {
        [Key]
        public int Id { get; set; }

        public string FKLogin { get; set; }

        [Required(ErrorMessage = "Nome é obrigatorio")]
        [StringLength(maximumLength: 150, MinimumLength = 5, ErrorMessage = "Nome precisa ter no minimo 5 caracteres e no maximo 150.")]
        public string? Nome { get; set; }

        public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public IEnumerable<Produto>? Produtos { get; set; }
    }
}
