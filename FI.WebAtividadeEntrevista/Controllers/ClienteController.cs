using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, errors));
            }

            BoCliente bo = new BoCliente();

            if (bo.VerificarExistencia(model.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado no sistema.");
            }

            var cliente = new Cliente()
            {
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                CPF = model.CPF?.Replace(".", "").Replace("-", "").Trim(),
                Telefone = model.Telefone
            };

            model.Id = bo.Incluir(cliente);

            if (model.Beneficiarios != null && model.Beneficiarios.Any())
            {
                var boBeneficiario = new FI.AtividadeEntrevista.BLL.BoBeneficiario();
                foreach (var ben in model.Beneficiarios)
                {
                    if (!string.IsNullOrEmpty(ben.CPF) && !string.IsNullOrEmpty(ben.Nome))
                    {
                        var beneficio = new FI.AtividadeEntrevista.DML.Beneficiario
                        {
                            IdCliente = model.Id,
                            CPF = ben.CPF,
                            Nome = ben.Nome
                        };
                        boBeneficiario.Incluir(beneficio);
                    }
                }
            }

            return Json("Cadastro efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            BoCliente bo = new BoCliente();

            if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado para outro cliente.");
            }

            bo.Alterar(new Cliente()
            {
                Id = model.Id,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                CPF = model.CPF?.Replace(".", "").Replace("-", "").Trim(),
                Telefone = model.Telefone
            });

            var boBeneficiario = new FI.AtividadeEntrevista.BLL.BoBeneficiario();

            var beneficiariosAtuais = boBeneficiario.ConsultarPorCliente(model.Id);

            var idsFront = model.Beneficiarios?.Where(b => b.Id > 0).Select(b => b.Id).ToList() ?? new List<long>();

            foreach (var ben in beneficiariosAtuais)
            {
                if (!idsFront.Contains(ben.Id))
                {
                    boBeneficiario.Excluir(ben.Id);
                }
            }

            if (model.Beneficiarios != null)
            {
                foreach (var ben in model.Beneficiarios)
                {
                    if (ben.Id > 0)
                    {
                        var beneficio = new FI.AtividadeEntrevista.DML.Beneficiario
                        {
                            Id = ben.Id,
                            IdCliente = model.Id,
                            CPF = ben.CPF,
                            Nome = ben.Nome
                        };
                        boBeneficiario.Alterar(beneficio);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ben.CPF) && !string.IsNullOrEmpty(ben.Nome))
                        {
                            var beneficio = new FI.AtividadeEntrevista.DML.Beneficiario
                            {
                                IdCliente = model.Id,
                                CPF = ben.CPF,
                                Nome = ben.Nome
                            };
                            boBeneficiario.Incluir(beneficio);
                        }
                    }
                }
            }

            return Json("Cadastro alterado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);

            if (cliente == null)
            {
                System.Diagnostics.Debug.WriteLine("Cliente não encontrado!");
                return HttpNotFound();
            }

            System.Diagnostics.Debug.WriteLine($"Cliente encontrado: {cliente.Nome}");

            var model = new ClienteModel()
            {
                Id = cliente.Id,
                CEP = cliente.CEP,
                Cidade = cliente.Cidade,
                Email = cliente.Email,
                Estado = cliente.Estado,
                Logradouro = cliente.Logradouro,
                Nacionalidade = cliente.Nacionalidade,
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                CPF = cliente.CPF,
                Telefone = cliente.Telefone,
                Beneficiarios = new List<BeneficiarioModel>()
            };

            try
            {
                var boBeneficiario = new FI.AtividadeEntrevista.BLL.BoBeneficiario();
                var beneficiarios = boBeneficiario.ConsultarPorCliente(id);


                if (beneficiarios != null && beneficiarios.Any())
                {
                    model.Beneficiarios = beneficiarios.Select(b => new BeneficiarioModel
                    {
                        Id = b.Id,
                        IdCliente = b.IdCliente,
                        CPF = b.CPF,
                        Nome = b.Nome
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" Erro ao carregar beneficiários: {ex.Message}");
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // ============================================
        // MÉTODOS PARA BENEFICIÁRIOS
        // ============================================

        [HttpGet]
        public JsonResult ListarBeneficiarios(long idCliente)
        {
            try
            {
                var bo = new FI.AtividadeEntrevista.BLL.BoBeneficiario();
                var beneficiarios = bo.ConsultarPorCliente(idCliente);

                var result = beneficiarios.Select(b => new
                {
                    Id = b.Id,
                    IdCliente = b.IdCliente,
                    CPF = b.CPF,
                    Nome = b.Nome
                });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult VerificarBeneficiarioExistente(string cpf, long? idCliente = null)
        {
            try
            {
                var bo = new FI.AtividadeEntrevista.BLL.BoBeneficiario();
                var resultado = bo.VerificarBeneficiarioExistente(cpf, idCliente);
                return Json(new { existe = resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { existe = false, erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Exclui um cliente
        /// </summary>
        [HttpPost]
        public JsonResult Excluir(long id)
        {
            try
            {
                var bo = new BoCliente();

                //  PRIMEIRO EXCLUI OS BENEFICIÁRIOS
                var boBeneficiario = new FI.AtividadeEntrevista.BLL.BoBeneficiario();
                var beneficiarios = boBeneficiario.ConsultarPorCliente(id);
                foreach (var ben in beneficiarios)
                {
                    boBeneficiario.Excluir(ben.Id);
                }

                //  DEPOIS EXCLUI O CLIENTE
                bo.Excluir(id);

                return Json("Cliente excluído com sucesso");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
        }
    }
}