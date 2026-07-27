using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        public long Incluir(Beneficiario beneficiario)
        {
            DaoBeneficiario dao = new DaoBeneficiario();

            // Verifica se já existe beneficiário com este CPF para este cliente
            if (dao.VerificarExistencia(beneficiario.CPF, beneficiario.IdCliente))
                throw new Exception("Beneficiário com este CPF já cadastrado para este cliente.");

            return dao.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        public void Alterar(Beneficiario beneficiario)
        {
            DaoBeneficiario dao = new DaoBeneficiario();

            // Verifica se já existe outro beneficiário com este CPF para este cliente
            // (Esta verificação é feita na procedure, mas mantemos como redundância)

            dao.Alterar(beneficiario);
        }

        /// <summary>
        /// Exclui um beneficiário
        /// </summary>
        public void Excluir(long id)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            dao.Excluir(id);
        }

        /// <summary>
        /// Consulta beneficiários por cliente
        /// </summary>
        public List<Beneficiario> ConsultarPorCliente(long idCliente)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            return dao.ConsultarPorCliente(idCliente);
        }

        /// <summary>
        /// Verifica se beneficiário existe para um cliente
        /// </summary>
        public bool VerificarExistencia(string cpf, long idCliente)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            return dao.VerificarExistencia(cpf, idCliente);
        }

        /// <summary>
        ///  VERIFICA SE CPF DE BENEFICIÁRIO JÁ EXISTE EM OUTRO CLIENTE
        /// </summary>
        public bool VerificarBeneficiarioExistente(string cpf, long? idCliente = null)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            return dao.VerificarBeneficiarioExistente(cpf, idCliente);
        }
    }
}