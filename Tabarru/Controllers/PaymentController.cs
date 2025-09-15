using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("save")]
        public async Task<Response> SavePayment([FromBody] PaymentSaveRequest dto)
        {
            return await paymentService.SavePayment(dto.MapToDto());

        }
    }
}
