using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tabarru.Common.Enums;

namespace Tabarru.Repositories.Models
{
    public class PaymentDetail : EntityMetaData
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string CampaignId { get; set; }

        [Required]
        public string PaymentId { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        public DateTime PaymentDateTime { get; set; }
        public string CustomerId { get; set; }
        public string AuthorizationCode { get; set; }
        public string PaymentMethodId { get; set; }
        public string Description { get; set; }
        public bool IsGiftAid { get; set; }
        public bool IsBankFeeCovered { get; set; }
        public bool IsRecurringPayment { get; set; }
    }
}
