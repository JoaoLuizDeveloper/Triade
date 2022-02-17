using System.Threading.Tasks;
using Triade.Models;

namespace Triade.Repository.IRepository
{
    public interface IProdutosRepository : IRepository<Produtos>
    {
        //Task<bool> AddDeveloperToProject(int developerId, int projectId);
    }
}