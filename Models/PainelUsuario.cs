using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFinanceFy.Models
{
    [Table("fin_painel_usuarios")]
    public class PainelUsuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string IdUsuario { get; set; } = null!;
        public string IdPainel { get; set; } = null!;
        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario? Usuario { get; set; }
        [ForeignKey(nameof(IdPainel))]
        public virtual Painel? Painel { get; set; }
    }
}
