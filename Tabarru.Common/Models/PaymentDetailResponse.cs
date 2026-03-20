using Tabarru.Common.Enums;

namespace Tabarru.Common.Models
{
    public class PaymentDetailResponse
    {
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string VendorType { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string Description { get; set; }
        public bool IsGiftAid { get; set; }
        public bool IsBankFeeCovered { get; set; }
        public bool IsRecurringPayment { get; set; }
    }
}
