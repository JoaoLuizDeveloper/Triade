using System.Threading.Tasks;
using Triade.Data;
using Triade.Models;
using Triade.Repository;
using Triade.Repository.IRepository;

namespace LubyTechAPI.Repository
{
    public class RetiradosRepository : Repository<Retirados> , IRetiradosRepository
    {
        #region Constructor
        public RetiradosRepository(ApplicationDbContext db) : base(db)
        {
        }
        #endregion
    }
}