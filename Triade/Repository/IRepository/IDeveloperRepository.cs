using LubyTechModel.Models;
using LubyTechModel.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LubyTechAPI.Repository.IRepository
{
    public interface IDeveloperRepository : IRepository<Developer>
    {
        Task<ICollection<HourByDeveloper>> GetRankinfOfDevelopers();
        Task<bool> CPFExists(long cpf);
        Task<bool> AddHourToProject(Hour Hour);

        string GetToken();
    }
}