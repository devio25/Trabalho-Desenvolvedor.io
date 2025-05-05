using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoLojaVirtualLibrary.Models
{
    public class Register
    {
        [Required(ErrorMessage = "E-mail é obrigatorio.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Nome é obrigatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Senha é obrigatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirme sua senha.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Senhas precisam ser identicas")]
        public string ConfirmPassword { get; set; }
    }
}
