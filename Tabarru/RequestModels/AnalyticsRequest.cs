using Microsoft.AspNetCore.Authorization;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class AnalyticsRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }


    static class AnalyticsExtension
    {
        public static AnalyticsDetailsDto MapToDto(this AnalyticsRequest analyticsRequest, string CharityId)
        {
            return new AnalyticsDetailsDto
            {
                CharityId = CharityId,
                StartDate = analyticsRequest.StartDate,
                EndDate = analyticsRequest.EndDate
            };
        }
    }
}
