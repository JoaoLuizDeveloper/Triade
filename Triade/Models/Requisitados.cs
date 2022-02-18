using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Triade.Models
{
    public class Requisitados
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "A quantidade retirada do produto é obrigatória", AllowEmptyStrings = false)]
        [Display(Name = "Informe o quantidade do produto retirada")]
        public int QtdRequisitada { get; set; }

        [Range(typeof(DateTime), "01/01/1900", "01/01/2023",
        ErrorMessage = "Data Requisitada {0} Precisa ser entre {1} e {2}")]
        [Display(Name = "Informe a Data Requisitada")]
        public DateTime DataRequisitada { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produtos Produto { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}