using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiário
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        internal long Incluir(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiario.IdCliente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("NOME", beneficiario.Nome));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        internal void Alterar(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiario.Id));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiario.IdCliente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("NOME", beneficiario.Nome));

            base.Executar("FI_SP_AltBeneficiario", parametros);
        }

        /// <summary>
        /// Exclui um beneficiário
        /// </summary>
        internal void Excluir(long id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        /// <summary>
        /// Consulta beneficiários por ID do cliente
        /// </summary>
        internal List<Beneficiario> ConsultarPorCliente(long idCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", idCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            return Converter(ds);
        }

        /// <summary>
        /// Verifica se beneficiário existe para um cliente
        /// </summary>
        internal bool VerificarExistencia(string cpf, long idCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", idCliente));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);
            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Converte DataSet para Lista de Beneficiario
        /// </summary>
        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario ben = new Beneficiario();
                    ben.Id = row.Field<long>("ID");
                    ben.IdCliente = row.Field<long>("IDCLIENTE");
                    ben.CPF = row.Field<string>("CPF");
                    ben.Nome = row.Field<string>("NOME");
                    lista.Add(ben);
                }
            }

            return lista;
        }

        /// <summary>
        ///  VERIFICA SE CPF DE BENEFICIÁRIO JÁ EXISTE EM OUTRO CLIENTE
        /// </summary>
        internal bool VerificarBeneficiarioExistente(string cpf, long? idCliente = null)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cpf));

            string procedure = "FI_SP_VerificaBeneficiarioExistente";

            if (idCliente.HasValue)
            {
                parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", idCliente.Value));
                procedure = "FI_SP_VerificaBeneficiarioExistenteComCliente";
            }

            DataSet ds = base.Consultar(procedure, parametros);
            return ds.Tables[0].Rows.Count > 0;
        }
    }
}