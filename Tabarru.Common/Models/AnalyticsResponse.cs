namespace Tabarru.Common.Models
{
    public class AnalyticsDataResponse
    {
        public GiftAidSmallDonationsScheme GiftAidSmallDonationsSchemes { get; set; }
        public List<CampaignDonationSummary> CampaignSummaries { get; set; } = new();
        public RevenueDashboardGraphResponse RevenueGraph { get; set; } = new();

    }

    public class RevenueGraphPoint
    {
        public string Label { get; set; }
        public decimal Revenue { get; set; }          // IsGiftAid = false
        public decimal GiftAidRevenue { get; set; }   // IsGiftAid = true
    }


    public class RevenueDashboardGraphResponse
    {
        public List<RevenueGraphPoint> Monthly { get; set; }

        public List<RevenueGraphPoint> Weekly { get; set; }

        public List<RevenueGraphPoint> Today { get; set; }

        public List<RevenueGraphPoint> Custom { get; set; }
    }

    public class CampaignDonationSummary
    {
        public string CampaignId { get; set; }
        public string CampaignName { get; set; }

        public decimal TodayAmount { get; set; }
        public decimal ThisWeekAmount { get; set; }
        public decimal ThisMonthAmount { get; set; }
        public decimal CustomAmount { get; set; }
    }


    public class GiftAidSmallDonationsScheme
    {
        public int TotalDonors { get; set; }
        public int TotalDonation { get; set; }
        public int GiftAndDonations { get; set; }
        public int GiftSmallDonations { get; set; }
    }
}
