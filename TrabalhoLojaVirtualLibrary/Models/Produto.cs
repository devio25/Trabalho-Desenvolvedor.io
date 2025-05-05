using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class Produto
    {
        [SwaggerIgnore]
        [Key]
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public decimal Valor { get; set; }

        public int Estoque { get; set; }

        [SwaggerIgnore]
        [JsonIgnore]
        public string? Imagem { get; set; }

        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        public int VendedorId { get; set; }

        [JsonIgnore]
        public Vendedor? Vendedor { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "A imagem do produto é obrigatória")]
        public IFormFile? Upload { get; set; }
    }
}
