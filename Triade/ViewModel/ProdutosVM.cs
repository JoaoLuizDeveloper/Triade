using System.ComponentModel.DataAnnotations;

namespace Triade.ViewModels
{
    public class ProdutosVM
    {
        public int Id { get; set; }
        
        public string NomeProduto { get; set; }

        public double PrecoCusto { get; set; }

        public double PrecoVenda { get; set; }

        public bool IsSimple { get; set; }

        public int Qtdproduto { get; set; }
        public int QtdRequisitadaOuRetirada { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
