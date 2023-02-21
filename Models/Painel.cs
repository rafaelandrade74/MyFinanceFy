using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinanceFy.Models
{
    [Table("fin_paineis")]
    public class Painel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Nome { get; set; }  = null!;
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string IdUsuario { get; set; }  = null!;
        [InverseProperty("Painel")]
        public virtual IEnumerable<PainelDados>? PainelDados { get; set; }
        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario? Usuario { get; set; }
    }
}