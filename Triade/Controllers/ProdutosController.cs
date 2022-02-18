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
    public class ProdutosController : Controller
    {
        #region Construtor
        private readonly IProdutosRepository _produtosRepository;
        private readonly IRequisitadosRepository _requisitadosRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public ProdutosController(IProdutosRepository produtosRepository, IRequisitadosRepository requisitadosRepository, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _produtosRepository = produtosRepository;
            _requisitadosRepository = requisitadosRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion

        #region Crud Create/Read/Update/Delete
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _produtosRepository.GetAll().Result.ToList() });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Produtos());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Produtos produto)
        {
            produto.Created = DateTime.Now;

            if (ModelState.IsValid)
            {
                var adding = await _produtosRepository.Add(produto);

                if (adding == true)
                {
                    return Json(new { success = true, message = "Adicionado com Sucesso" });
                }
                else
                {
                    return Json(new { success = false, message = "Falha ao adicionar. Verifique os campos!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Falha ao adicionar. Verifique os campos!" });
            }                       
        }
        
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var model = await _produtosRepository.Get(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Produtos produto)
        {
            if (ModelState.IsValid)
            {
                produto.Updated = DateTime.Now;
                var adding = await _produtosRepository.Update(produto);

                if (adding == true)
                {
                    return Json(new { success = true, message = "Atualizado com Sucesso" });
                }
                else
                {
                    return Json(new { success = false, message = "Falha ao Atualizar. Verifique os campos!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Falha ao Atualizar. Verifique os campos!" });
            }                       
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _produtosRepository.Remove(id);
            if (objFromDB == null || objFromDB.Result == false)
            {
                return Json(new { success = false, message = "Erro enquanto estava deletando" });
            }
            return Json(new { success = true, message = "Deletado com Sucesso" });
        }
        #endregion

        #region Retirar Produto
        [HttpGet]
        public async Task<IActionResult> RequisitarProduto(int id)
        {
            var model = _mapper.Map<ProdutosVM>(await _produtosRepository.Get(id));

            model.QtdRequisitadaOuRetirada = 0;

            return Json(new { model });
        }

        [HttpPost]
        public async Task<IActionResult> RequisitarProduto(ProdutosVM produtovm)
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
                    
                    var retirado = new Requisitados()
                    {
                        ProdutoId = produto.Id,
                        QtdRequisitada = produtovm.QtdRequisitadaOuRetirada,
                        UserId = user.Id,
                        DataRequisitada = DateTime.Now,
                        Created = DateTime.Now,
                    };

                    await _requisitadosRepository.Update(retirado);

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
        
        [HttpGet]
        public async Task<IActionResult> AdicionarQtdProduto(int id)
        {
            var model = _mapper.Map<ProdutosVM>(await _produtosRepository.Get(id));

            model.QtdRequisitadaOuRetirada = 0;

            return Json(new { model });
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarQtdProduto(ProdutosVM produtovm)
        {
            if (ModelState.IsValid)
            {
                produtovm.Updated = DateTime.Now;
                var produto = _mapper.Map<Produtos>(produtovm);

                produto.Qtdproduto += produtovm.QtdRequisitadaOuRetirada;

                var updatingQtd = await _produtosRepository.Update(produto);

                if (updatingQtd == true)
                {
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