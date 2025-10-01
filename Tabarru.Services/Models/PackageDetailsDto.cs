using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{
    public class PackageDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string? BillingCycle { get; set; }
        public List<string> Features { get; set; }
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
