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
        private readonly DbStorageContext dbContext;

        public PaymentService(IPaymentRepository paymentRepository,
            IRecurringPaymentRepository recurringPaymentRepository,
            IGiftAidRepository giftAidRepository,
             DbStorageContext dbContext)
        {
            this.paymentRepository = paymentRepository;
            this.recurringPaymentRepository = recurringPaymentRepository;
            this.giftAidRepository = giftAidRepository;
            this.dbContext = dbContext;
        }

        public async Task<Response> SavePayment(PaymentDto paymentDto)
        {

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var paymentDetail = new PaymentDetail
                    {
                        PaymentId = paymentDto.PaymentId,
                        Amount = paymentDto.Amount,
                        Currency = paymentDto.Currency,
                        Status = paymentDto.Status,
                        PaymentDateTime = paymentDto.PaymentDateTime,
                        CustomerId = paymentDto.CustomerId,
                        AuthorizationCode = paymentDto.AuthorizationCode,
                        IsGiftAid = paymentDto.IsGiftAid,
                        IsBankFeeCovered = paymentDto.IsBankFeeCovered,
                        IsRecurringPayment = paymentDto.IsRecurringPayment,
                        PaymentMethodId = paymentDto.PaymentMethodId
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
                            PaymentId = paymentDto.PaymentId,
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
    }
}
