using System;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de beneficiário que representa o registro na tabela BENEFICIARIOS do Banco de Dados
    /// </summary>
    public class Beneficiario
    {
        /// <summary>
        /// ID do beneficiário
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID do cliente (FK)
        /// </summary>
        public long IdCliente { get; set; }

        /// <summary>
        /// CPF do beneficiário
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Nome do beneficiário
        /// </summary>
        public string Nome { get; set; }
    }
}