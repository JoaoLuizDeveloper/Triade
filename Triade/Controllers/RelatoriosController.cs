using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Triade.Models;
using Triade.Repository.IRepository;
using Triade.ViewModels;

namespace Triade.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        #region Construtor
        private readonly IProdutosRepository _produtosRepository;
        private readonly IRetiradosRepository _retiradosRepository;
        private readonly IRequisitadosRepository _requisitadosRepository;
        private readonly IMapper _mapper;
        public RelatoriosController(IProdutosRepository produtosRepository, IRetiradosRepository retiradosRepository, IRequisitadosRepository requisitadosRepository, IMapper mapper)
        {
            _produtosRepository = produtosRepository;
            _retiradosRepository = retiradosRepository;
            _requisitadosRepository = requisitadosRepository;
            _mapper = mapper;
        }
        #endregion
                
        public IActionResult SaidasEstoque()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllSaidasEstoque()
        {
            return Json(new { data = _retiradosRepository.GetAll().Result.ToList() });
        }

        public IActionResult Requisicoes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllRequisicoes()
        {
            return Json(new { data = _requisitadosRepository.GetAll().Result.ToList() });
        }


        #region Retirar Produto
        [HttpGet]
        public async Task<IActionResult> RetirarProduto(int id)
        {
            var model = _mapper.Map<ProdutosVM>(await _produtosRepository.Get(id));

            model.QtdRequisitadaOuRetirada = 0;

            return Json(new { model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetirarProduto(ProdutosVM produtovm)
        {
            if (ModelState.IsValid)
            {
                produtovm.Updated = DateTime.Now;
                var produto = _mapper.Map<Produtos>(produtovm);

                produto.Qtdproduto -= produtovm.QtdRequisitadaOuRetirada;

                var updatingQtd = await _produtosRepository.Update(produto);

                if (updatingQtd == true)
                {
                    var retirado = new Retirados()
                    {
                        ProdutoId = produto.Id,
                        QtdRetirada = produtovm.QtdRequisitadaOuRetirada,
                        UserId = 0,
                        DataRetirada = DateTime.Now,
                        Created = DateTime.Now,
                    };

                    await _retiradosRepository.Update(retirado);

                    return Json(new { success = true, message = "Quantidade atualizada com Sucesso" });
                }
                else
                {
                    return Json(new { success = false, message = "Falha ao atualizar quantidade. Verifique os campos!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Falha ao atualizar quantidade. Verifique os campos!" });
            }
        }
        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}