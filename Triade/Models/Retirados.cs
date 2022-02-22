using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triade.Models
{
    public class Retirados
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "A quantidade retirada do produto é obrigatória", AllowEmptyStrings = false)]
        [Display(Name = "Informe o quantidade do produto retirada")]
        public int QtdRetirada { get; set; }

        [Range(typeof(DateTime), "01/01/1900", "01/01/2023",
        ErrorMessage = "Data de Retirada {0} Precisa ser entre {1} e {2}")]
        [Display(Name = "Informe a Data de Retirada")]
        public DateTime DataRetirada { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produtos Produto { get; set; }

        //[Required]
        //public int RequisicaoId { get; set; }

        //[ForeignKey("RequisicaoId")]
        //public Requisitados Requisitado { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
