using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface ICharityAccountService
    {
        Task<ResultData> Register(CharityDetailDto dto);
        Task<CharityDetailDto> GetByEmail(string email);
        Task<Response<LoginResponse>> Login(LoginDto dto);
    }
}
