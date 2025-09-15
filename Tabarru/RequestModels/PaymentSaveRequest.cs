using Tabarru.Common.Enums;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class PaymentSaveRequest
    {
        public string PaymentId { get; set; }
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
        public static PaymentDto MapToDto(this PaymentSaveRequest userDetail)
        {
            return new PaymentDto
            {
                PaymentId = userDetail.PaymentId,
                Amount = userDetail.Amount,
                AuthorizationCode = userDetail.AuthorizationCode,
                Currency = userDetail.Currency,
                CustomerId = userDetail.CustomerId,
                IsBankFeeCovered = userDetail.IsBankFeeCovered,
                IsGiftAid = userDetail.IsGiftAid,
                IsRecurringPayment = userDetail.IsRecurringPayment,
                NextRecurringDate = userDetail.NextRecurringDate,
                PaymentDateTime = userDetail.PaymentDateTime,
                PaymentMethodId = userDetail.PaymentMethodId,
                Status = userDetail.Status,
                GiftAid = userDetail.GiftAid.MapToDto()
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
