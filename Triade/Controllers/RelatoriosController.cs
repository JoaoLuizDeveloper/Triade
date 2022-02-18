using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public RelatoriosController(IProdutosRepository produtosRepository, IRetiradosRepository retiradosRepository, IRequisitadosRepository requisitadosRepository, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _produtosRepository = produtosRepository;
            _retiradosRepository = retiradosRepository;
            _requisitadosRepository = requisitadosRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion
                
        public IActionResult SaidasEstoque()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSaidasEstoque()
        {
            var model = await _retiradosRepository.GetAll().ConfigureAwait(true);
            return Json(new { data = model });
        }

        public IActionResult Requisicoes()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequisicoes()
        {
            var model = await _requisitadosRepository.GetAll().ConfigureAwait(true);
            return Json(new { data = model });
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
                    var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(true);
                    var retirado = new Retirados()
                    {
                        ProdutoId = produto.Id,
                        QtdRetirada = produtovm.QtdRequisitadaOuRetirada,
                        UserId = user.Id,
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