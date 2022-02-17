using LubyTechAPI.Repository;
using Microsoft.Extensions.DependencyInjection;
using Triade.Repository.IRepository;

namespace Triade.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProdutosRepository, ProdutosRepository>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IProjectRepository, ProjectRepository>();
            return services;        
        }
    }
}
