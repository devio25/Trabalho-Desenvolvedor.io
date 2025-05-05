using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class InserirNovoProduto
    {
        [Required(ErrorMessage = "O produto precida ter uma descrição.")]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "O campo descrição precisa ter no minimo 5 caracteres e no maximo 50.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo valor é obrigatório.")]
        
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo de estoque é obrigatorio")]
        [Range(1, 9999, ErrorMessage = "O campo deve contter um valor entre 1 e 9999")]
        public int Estoque { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "A imagem do produto é obrigatória")]
        public IFormFile? ImagemUpload { get; set; }


        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }
    }
}