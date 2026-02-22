namespace Tabarru.Common.Models
{
    public class DonationReportResponse
    {
        public string DonorId { get; set; }
        public DateTime LatestDonationDate { get; set; }
        public bool? GiftAid { get; set; } // nullable for reports where GiftAid isn't relevant
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public decimal Donation { get; set; }
    }
}
