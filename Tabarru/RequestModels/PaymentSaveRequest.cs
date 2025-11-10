using Tabarru.Common.Enums;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class PaymentSaveRequest
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string CustomerId { get; set; }
        public string AuthorizationCode { get; set; }
        public bool IsGiftAid { get; set; }
        public bool IsBankFeeCovered { get; set; }
        public bool IsRecurringPayment { get; set; }
        public string Description { get; set; }
        public string VendorType { get; set; }
        public string PaymentMethodId { get; set; }
        public string TemplateId { get; set; }
        public string CampaignId { get; set; }
        // GiftAid details
        public GiftAidRequest GiftAid { get; set; }

        // Recurring Payment details
        public DateTime? NextRecurringDate { get; set; }
    }

    public class GiftAidRequest
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
    }

    static class PaymentDetailExtension
    {
        public static PaymentDto MapToDto(this PaymentSaveRequest paymentSaveRequest, string CharityId)
        {
            return new PaymentDto
            {
                TransactionId = paymentSaveRequest.TransactionId,
                Amount = paymentSaveRequest.Amount,
                AuthorizationCode = paymentSaveRequest.AuthorizationCode,
                Currency = paymentSaveRequest.Currency,
                CustomerId = paymentSaveRequest.CustomerId,
                IsBankFeeCovered = paymentSaveRequest.IsBankFeeCovered,
                IsGiftAid = paymentSaveRequest.IsGiftAid,
                IsRecurringPayment = paymentSaveRequest.IsRecurringPayment,
                NextRecurringDate = paymentSaveRequest.NextRecurringDate,
                PaymentDateTime = paymentSaveRequest.PaymentDateTime,
                PaymentMethodId = paymentSaveRequest.PaymentMethodId,
                Status = paymentSaveRequest.Status,
                CampaignId = paymentSaveRequest.CampaignId,
                Description = paymentSaveRequest.Description,
                TemplateId = paymentSaveRequest.TemplateId,
                VendorType = paymentSaveRequest.VendorType,
                GiftAid = paymentSaveRequest.GiftAid.MapToDto(),
                CharityId = CharityId
            };
        }

        public static GiftAidDto MapToDto(this GiftAidRequest giftAidRequest)
        {
            return new GiftAidDto
            {
                Address = giftAidRequest.Address,
                FirstName = giftAidRequest.FirstName,
                Phone = giftAidRequest.Phone,
                Postcode = giftAidRequest.Postcode,
                Surname = giftAidRequest.Surname,
                Title = giftAidRequest.Title
            };


        }

    }
}
