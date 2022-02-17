using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Triade.Models;
using Triade.Repository.IRepository;

namespace Triade.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly IProdutosRepository _produtosRepository;

        public ProdutosController(IProdutosRepository produtosRepository)
        {
            _produtosRepository = produtosRepository;
        }

        public IActionResult Index()
        {
            //var model = _produtosRepository.GetAll();
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _produtosRepository.GetAll().Result.ToList();
            return Json(new { data = model });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Produtos();

            return View(model);
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
            produto.Created = DateTime.Now;

            if (ModelState.IsValid)
            {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}