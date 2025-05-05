using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class Login
    {
        [Required(ErrorMessage = "E-mail é obrigatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Digite um e-mail válido")]
        [EmailAddress(ErrorMessage = "Entre com um e-mail válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatoria.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
