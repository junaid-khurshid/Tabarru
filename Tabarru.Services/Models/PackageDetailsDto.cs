using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{
    public class PackageDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string? BillingCycle { get; set; }
        public List<string> Features { get; set; } = new();
        public DateTimeOffset CreatedDate { get; set; }
    }

    static class PackageDetailsExtension
    {
        public static PackageDetailsDto MapToDto(this PackageDetails p)
        {
            return new PackageDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                BillingCycle = p.BillingCycle,
                Features = p.Features,
                CreatedDate = p.CreatedDate
            };
        }
    }
}
