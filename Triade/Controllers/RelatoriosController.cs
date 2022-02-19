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
            return Json(new { data = await _retiradosRepository.GetAll(includeProperties: "Produto").ConfigureAwait(true) });
        }

        public IActionResult Requisicoes()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequisicoes()
        {
            return Json(new { data = await _requisitadosRepository.GetAll(includeProperties: "Produto").ConfigureAwait(true) });
        }


        #region Retirar Produto
        [HttpGet]
        public async Task<IActionResult> RetirarRequisicao(int id)
        {
            var model = _mapper.Map<Requisitados>(await _requisitadosRepository.GetFirstOrDefault(x=> x.Id == id, includeProperties: "Produto"));

            return Json(new { model });
        }

        [HttpPost]
        public async Task<IActionResult> RetirarRequisicao(RetiradosRequisicaoVM retiradoRequisicao)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(true);

                var retirado = new Retirados()
                {
                    DataRetirada = DateTime.Now,
                    Created = DateTime.Now,
                    UserId = user.Id,
                    ProdutoId = retiradoRequisicao.ProdutoId,
                    QtdRetirada = retiradoRequisicao.QtdRetirada
                };

                await _retiradosRepository.Update(retirado);

                await _requisitadosRepository.Remove(retiradoRequisicao.RequisicaoId);

                return Json(new { success = true, message = "Requisição finalizada com Sucesso" });
            }
            else
            {
                return Json(new { success = false, message = "Falha ao finalizar requisição!" });
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