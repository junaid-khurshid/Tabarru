namespace Tabarru.Repositories.Models
{
    public class PackageDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string BillingCycle { get; set; }
        public List<string> Features { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}