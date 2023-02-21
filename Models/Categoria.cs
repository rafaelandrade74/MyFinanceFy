using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinanceFy.Models
{
    [Table("fin_categorias")]
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Cor { get; set; }
    }
}