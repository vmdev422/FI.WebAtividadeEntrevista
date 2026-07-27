using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAtividadeEntrevista.Models.Validation;

namespace WebAtividadeEntrevista.Models
{
    public class ClienteModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória")]
        public string Cidade { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        [MaxLength(2, ErrorMessage = "Estado deve ter 2 caracteres")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Logradouro é obrigatório")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Nacionalidade é obrigatória")]
        public string Nacionalidade { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
        [CPFValidation(ErrorMessage = "CPF inválido.")] 
        public string CPF { get; set; }

        public string Telefone { get; set; }

        // LISTA DE BENEFICIÁRIOS
        public List<BeneficiarioModel> Beneficiarios { get; set; } = new List<BeneficiarioModel>();
    }
}