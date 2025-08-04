using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IEmailVerificationRepository
    {
        Task<EmailVerificationDetails> GetByIdAsync(string emailVerificationId);
        Task<EmailVerificationDetails> GetByEmailAsync(string emailAddress);
        Task<bool> AddAsync(EmailVerificationDetails emailVerificationDetails);
        Task<bool> UpdateAsync(EmailVerificationDetails emailVerificationDetails);
        Task<EmailVerificationDetails> GetByEmailAndIsNotUsedAsync(string emailAddress);
    }
}
