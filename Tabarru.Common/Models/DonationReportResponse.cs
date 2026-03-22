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

    public class StudentFormTransactionResponse
    {
        // Payment fields
        public string PaymentId { get; set; }
        public string DonorId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public decimal Donation { get; set; }

        // Student Form fields
        public string StudentName { get; set; }
        public string ParentName { get; set; }
        public string FullAddress { get; set; }
        public string StudentId { get; set; }
        public string ParentId { get; set; }
        public int? StudentAmount { get; set; }
        public string Period { get; set; }
        public string Notes { get; set; }

        // Optional descriptions
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
    }

    public class MembershipTransactionResponse
    {
        // Payment fields
        public string PaymentId { get; set; }
        public string DonorId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public decimal Donation { get; set; }

        // Membership fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HouseNumberAndName { get; set; }
        public string PostalCode { get; set; }

        // Extra descriptions
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
    }
}
