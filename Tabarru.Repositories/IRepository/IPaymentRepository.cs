﻿using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IPaymentRepository
    {
        Task<bool> AddAsync(PaymentDetail paymentDetail);
        Task<PaymentDetail> GetByIdAsync(string PaymentId);
        Task<bool> UpdateAsync(PaymentDetail paymentDetail);
    }
}
