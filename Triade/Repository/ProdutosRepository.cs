using System.Threading.Tasks;
using Triade.Data;
using Triade.Models;
using Triade.Repository;
using Triade.Repository.IRepository;

namespace LubyTechAPI.Repository
{
    public class ProdutosRepository : Repository<Produtos> , IProdutosRepository
    {
        #region Constructor
        public ProdutosRepository(ApplicationDbContext db) : base(db)
        {
        }
        #endregion
    }
}