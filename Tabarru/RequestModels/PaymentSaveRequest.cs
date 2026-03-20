using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tabarru.Common.Enums;
using Tabarru.Repositories.Models;
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
        public bool IsMemberShipForm { get; set; }
        public bool IsStudentForm { get; set; }
        public bool IsBankFeeCovered { get; set; }
        public bool IsRecurringPayment { get; set; }
        public string Description { get; set; }
        public string VendorType { get; set; }
        public string PaymentMethodId { get; set; }
        public string TemplateId { get; set; }
        public string CampaignId { get; set; }
        // GiftAid details
        public GiftAidRequest GiftAid { get; set; }
        public MembershipDetailRequest MembershipDetailRequest { get; set; }
        public StudentFormDetailRequest StudentFormDetailRequest { get; set; }

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

    public class StudentFormDetailRequest
    {
        public string StudentName { get; set; }
        public string ParentName { get; set; }
        public string FullAddress { get; set; }
        public string StudentId { get; set; }
        public string ParentId { get; set; }
        public int Amount { get; set; }
        public string Period { get; set; }
        public string Notes { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
    }

    public class MembershipDetailRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HouseNumberAndName { get; set; }
        public string PostalCode { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
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
                IsMemberShipForm = paymentSaveRequest.IsMemberShipForm,
                IsStudentForm = paymentSaveRequest.IsStudentForm,
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
                MembershipDetailDto = paymentSaveRequest.MembershipDetailRequest.MapToDto(),
                StudentFormDetailDto = paymentSaveRequest.StudentFormDetailRequest.MapToDto(),
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

        public static MembershipDetailDto MapToDto(this MembershipDetailRequest membershipDetailRequest)
        {
            return new MembershipDetailDto
            {
                FirstName = membershipDetailRequest.FirstName,
                LastName = membershipDetailRequest.LastName,
                HouseNumberAndName = membershipDetailRequest.HouseNumberAndName,
                PostalCode = membershipDetailRequest.PostalCode,
                Description1 = membershipDetailRequest.Description1,
                Description2 = membershipDetailRequest.Description2,
                Description3 = membershipDetailRequest.Description3
            };
        }

        public static StudentFormDetailDto MapToDto(this StudentFormDetailRequest studentFormDetailRequest)
        {
            return new StudentFormDetailDto
            {
                Amount = studentFormDetailRequest.Amount,
                Description3 = studentFormDetailRequest.Description3,
                Description2 = studentFormDetailRequest.Description2,
                Description1 = studentFormDetailRequest.Description1,
                FullAddress = studentFormDetailRequest.FullAddress,
                Notes = studentFormDetailRequest.Notes,
                ParentId = studentFormDetailRequest.ParentId,
                ParentName = studentFormDetailRequest.ParentName,
                Period = studentFormDetailRequest.Period,
                StudentId = studentFormDetailRequest.StudentId,
                StudentName = studentFormDetailRequest.StudentName
            };
        }
    }
}