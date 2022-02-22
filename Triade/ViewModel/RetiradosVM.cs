using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Triade.Models;

namespace Triade.ViewModels
{
    public class RetiradosVM
    {
        public int QtdRetirada { get; set; }

        public DateTime DataRetirada { get; set; }
                
        public Produtos Produto { get; set; }
        
        public virtual IdentityUser Usuario { get; set; }
    }
}
