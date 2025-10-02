using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class RecurringPaymentDetail : EntityMetaDataWithDeleteAble
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]

        public string PaymentDetailId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }

        public string PaymentId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethodInfo { get; set; }
        public string CustomerId { get; set; }
        public string AuthorizationCode { get; set; }
        public DateTime NextRecurringDate { get; set; }
    }
}
