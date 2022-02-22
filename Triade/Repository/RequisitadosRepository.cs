using System.Threading.Tasks;
using Triade.Data;
using Triade.Models;
using Triade.Repository;
using Triade.Repository.IRepository;

namespace LubyTechAPI.Repository
{
    public class RequisitadosRepository : Repository<Requisitados> , IRequisitadosRepository
    {
        #region Constructor
        public RequisitadosRepository(ApplicationDbContext db) : base(db)
        {
        }
        #endregion
    }
}