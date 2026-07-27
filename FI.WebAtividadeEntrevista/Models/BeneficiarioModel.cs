using System.ComponentModel.DataAnnotations;
using WebAtividadeEntrevista.Models.Validation;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        public long IdCliente { get; set; }

        [Required(ErrorMessage = "CPF do beneficiário é obrigatório")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
        [CPFValidation(ErrorMessage = "CPF do beneficiário inválido.")] 
        public string CPF { get; set; }

        [Required(ErrorMessage = "Nome do beneficiário é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }
    }
}