using AutoMapper;
using Triade.Models;
using Triade.ViewModels;

namespace Triade.Mapper
{
    public class TriadeMappings : Profile
    {
        public TriadeMappings()
        {
            CreateMap<Produtos, ProdutosVM>().ReverseMap();
        }
    }
}