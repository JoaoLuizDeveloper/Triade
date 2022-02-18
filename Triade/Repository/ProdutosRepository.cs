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
        
        //public async Task<bool> CreateAddDeveloperToProject(int developerId, int projectId)
        //{
        //    //await _db.Developers_Projects.AddAsync(new Developers_Projects() { DeveloperId = developerId, ProjectId = projectId });
        //    //return await _db.SaveChangesAsync() >= 0;
        //    return false;
        //}
    }
}