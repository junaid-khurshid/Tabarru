using Tabarru.Common.Enums;

namespace Tabarru.Services.Models
{
    public class PaymentDto
    {
        public string TransactionId { get; set; }
        public string CharityId { get; set; }
        public string TemplateId { get; set; }
        public string CampaignId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string CustomerId { get; set; }
        public string AuthorizationCode { get; set; }
        public bool IsGiftAid { get; set; }
        public bool IsBankFeeCovered { get; set; }
        public bool IsRecurringPayment { get; set; }
        public string PaymentMethodId { get; set; }
        public string Description { get; set; }

        public string VendorType { get; set; }

        // GiftAid details
        public GiftAidDto GiftAid { get; set; }

        // Recurring Payment details
        public DateTime? NextRecurringDate { get; set; }
    }

    public class GiftAidDto
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
    }

    public class PaymentReadDetailDto
    {
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string CampaignId { get; set; }
        public string TemplateId { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
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