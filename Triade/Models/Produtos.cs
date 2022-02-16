using System.ComponentModel.DataAnnotations;

namespace Triade.Models
{
    public class Produtos
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do produto é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Informe o produto")]
        [StringLength(150, MinimumLength = 4)]
        public string NomeProduto { get; set; }

        [Required(ErrorMessage = "O preço de custo do produto é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Informe o preço de custo do produto")]
        public double PrecoCusto { get; set; }

        [Required(ErrorMessage = "O preço de venda do produto é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Informe preço de venda do produto")]
        public double PrecoVenda { get; set; }

        public bool IsSimple { get; set; }
        [Required(ErrorMessage = "A quantidade do produto é obrigatória", AllowEmptyStrings = false)]
        [Display(Name = "Informe a quantidade do produto")]
        public int Qtdproduto { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
