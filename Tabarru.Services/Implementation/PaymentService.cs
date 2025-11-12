using Azure.Core;
using System.Net;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IRecurringPaymentRepository recurringPaymentRepository;
        private readonly IGiftAidRepository giftAidRepository;
        private readonly ITemplateRepository templateRepository;
        private readonly ICampaignRepository campaignRepository;
        private readonly DbStorageContext dbContext;

        public PaymentService(IPaymentRepository paymentRepository,
            IRecurringPaymentRepository recurringPaymentRepository,
            IGiftAidRepository giftAidRepository,
            ITemplateRepository templateRepository,
            ICampaignRepository campaignRepository,
             DbStorageContext dbContext)
        {
            this.paymentRepository = paymentRepository;
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.giftAidRepository = giftAidRepository;
            this.templateRepository = templateRepository;
            this.campaignRepository = campaignRepository;
            this.dbContext = dbContext;
        }

        public async Task<Response> SavePayment(PaymentDto paymentDto)
        {

            var template = await templateRepository.GetByIdAsync(paymentDto.TemplateId);
            if (template == null)
                return new Response(HttpStatusCode.NotFound, "Template details not found");

            var campaign = await campaignRepository.GetByIdAsync(paymentDto.CampaignId);
            if (campaign == null)
                return new Response(HttpStatusCode.BadRequest, "Campaign Details not found.");

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var paymentDetail = new PaymentDetail
                    {
                        Id = Guid.NewGuid().ToString(),
                        TransactionId = paymentDto.TransactionId,
                        Amount = paymentDto.Amount,
                        Currency = paymentDto.Currency,
                        Status = paymentDto.Status,
                        PaymentDateTime = paymentDto.PaymentDateTime,
                        CustomerId = paymentDto.CustomerId,
                        AuthorizationCode = paymentDto.AuthorizationCode,
                        IsGiftAid = paymentDto.IsGiftAid,
                        IsBankFeeCovered = paymentDto.IsBankFeeCovered,
                        IsRecurringPayment = paymentDto.IsRecurringPayment,
                        PaymentMethodId = paymentDto.PaymentMethodId,
                        Description = paymentDto.Description,
                        CharityId = paymentDto.CharityId,
                        CampaignId = paymentDto.CampaignId,
                        TemplateId = paymentDto.TemplateId,
                        VendorType = paymentDto.VendorType,
                    };

                    await paymentRepository.AddAsync(paymentDetail);

                    // Save GiftAid
                    if (paymentDto.IsGiftAid && paymentDto.GiftAid != null)
                    {
                        var giftAid = new GiftAidDetail
                        {
                            PaymentDetailId = paymentDetail.Id,
                            Title = paymentDto.GiftAid.Title,
                            FirstName = paymentDto.GiftAid.FirstName,
                            Surname = paymentDto.GiftAid.Surname,
                            Address = paymentDto.GiftAid.Address,
                            Postcode = paymentDto.GiftAid.Postcode,
                            Phone = paymentDto.GiftAid.Phone
                        };
                        await giftAidRepository.AddAsync(giftAid);
                    }

                    // Save RecurringPayment
                    if (paymentDto.IsRecurringPayment && paymentDto.NextRecurringDate.HasValue)
                    {
                        var recurring = new RecurringPaymentDetail
                        {
                            PaymentDetailId = paymentDetail.Id,
                            TransactionId = paymentDto.TransactionId,
                            Amount = paymentDto.Amount,
                            Currency = paymentDto.Currency,
                            CustomerId = paymentDto.CustomerId,
                            AuthorizationCode = paymentDto.AuthorizationCode,
                            NextRecurringDate = paymentDto.NextRecurringDate.Value
                        };
                        await recurringPaymentRepository.AddAsync(recurring);
                    }

                    await transaction.CommitAsync();
                    return new Response(HttpStatusCode.OK, "Payment Successfull");
                }

                catch (Exception ex)
                {
                    // Rollback if any error occurs
                    await transaction.RollbackAsync();
                    return new Response(HttpStatusCode.BadRequest, "Payment failed");
                    throw ex;
                }
            }

        }

        public async Task<Response> PaymentRecurring(PaymentDetail paymentDetail)
        {
            return new Response(HttpStatusCode.OK);
        }


        public async Task<Response<List<PaymentReadDetailDto>>> GetByCharityIdAsync(string charityId)
        {
            var paymentDetails = (await paymentRepository.GetByCharityIdAsync(charityId)).ToList();

            if (paymentDetails == null || paymentDetails.Count == 0)
                return new Response<List<PaymentReadDetailDto>>(HttpStatusCode.NotFound, "No payment details found for this charity.");

            var result = paymentDetails.Select(x => new PaymentReadDetailDto
            {
                Id = x.Id,
                CharityId = x.CharityId,
                CampaignId = x.CampaignId,
                TemplateId = x.TemplateId,
                TransactionId = x.TransactionId,
                Status = x.Status.ToString(),
                Amount = x.Amount,
                Currency = x.Currency,
                VendorType = x.VendorType,
                PaymentDateTime = x.PaymentDateTime,
                Description = x.Description,
                IsGiftAid = x.IsGiftAid,
                IsBankFeeCovered = x.IsBankFeeCovered,
                IsRecurringPayment = x.IsRecurringPayment
            }).ToList();

            return new Response<List<PaymentReadDetailDto>>(HttpStatusCode.OK, result, Common.Enums.ResponseCode.Data);
        }

        public async Task<Response<List<PaymentReadDetailDto>>> GetByCampaignOrTemplateIdAsync(string? campaignId, string? templateId)
        {
            if (string.IsNullOrEmpty(campaignId) && string.IsNullOrEmpty(templateId))
                return new Response<List<PaymentReadDetailDto>>(HttpStatusCode.BadRequest, "Either campaignId or templateId must be provided.");

            var paymentDetails = (await paymentRepository.GetByCampaignOrTemplateIdAsync(campaignId, templateId)).ToList();

            if (paymentDetails == null || paymentDetails.Count == 0)
                return new Response<List<PaymentReadDetailDto>>(HttpStatusCode.NotFound, "No payment details found for given criteria.");

            var result = paymentDetails.Select(x => new PaymentReadDetailDto
            {
                Id = x.Id,
                CharityId = x.CharityId,
                CampaignId = x.CampaignId,
                TemplateId = x.TemplateId,
                TransactionId = x.TransactionId,
                Status = x.Status.ToString(),
                Amount = x.Amount,
                Currency = x.Currency,
                VendorType = x.VendorType,
                PaymentDateTime = x.PaymentDateTime,
                Description = x.Description,
                IsGiftAid = x.IsGiftAid,
                IsBankFeeCovered = x.IsBankFeeCovered,
                IsRecurringPayment = x.IsRecurringPayment
            }).ToList();

            return new Response<List<PaymentReadDetailDto>>(HttpStatusCode.OK, result, Common.Enums.ResponseCode.Data);
        }
    }
}
