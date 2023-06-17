

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyFinanceFy.Libs.Enums;

namespace MyFinanceFy.Models
{
    [Table("fin_painel_dados")]
    public class PainelDados
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Descricao { get; set; } = null!;        
        public string? Observacao { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(1, 600, ErrorMessage = "o campo precisa ter entre {1} e {2}.")]
        public int Parcelas { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public TipoSaldo TipoSaldo { get; set; }
        [Display(Name = "Categoria")]
        public string IdCategoria { get; set; } = null!;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? DataPagamento { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorPago { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly DataFatura { get; set; }
        public DateTime DataCadastro { get; set; }  = DateTime.Now;
        [ForeignKey(nameof(IdCategoria))]
        public virtual Categoria? Categoria { get; set; }
        
        public string? IdPainel { get; set; }
        [ForeignKey(nameof(IdPainel))]
        public virtual Painel? Painel { get; set; }
    }
}