using LubyTechAPI.Repository;
using Triade.Repository.IRepository;

namespace Triade.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProdutosRepository, ProdutosRepository>();
            services.AddScoped<IRetiradosRepository, RetiradosRepository>();
            services.AddScoped<IRequisitadosRepository, RequisitadosRepository>();
            return services;        
        }
    }
}
