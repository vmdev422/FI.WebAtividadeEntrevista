using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models.Validation
{
    public class CPFValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string cpf = value.ToString().Replace(".", "").Replace("-", "").Replace("/", "").Trim();

            if (string.IsNullOrEmpty(cpf))
                return new ValidationResult("CPF é obrigatório.");

            if (cpf.Length != 11)
                return new ValidationResult("CPF deve ter 11 dígitos.");

            if (!IsCpfValid(cpf))
                return new ValidationResult(ErrorMessage ?? "CPF inválido.");

            return ValidationResult.Success;
        }

        private bool IsCpfValid(string cpf)
        {
            // Verifica se todos os dígitos são iguais
            if (new string(cpf[0], 11) == cpf)
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[9].ToString()) != digitoVerificador1)
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            if (int.Parse(cpf[10].ToString()) != digitoVerificador2)
                return false;

            return true;
        }
    }
}