namespace Tabarru.Services.Models
{
    public class CampaignUpdateStatusDto
    {
        public string CampaignId { get; set; }

        public string CharityId { get; set; }

        public bool isMembershipForm { get; set; }

        public bool isStudentForm { get; set; }
    }
}
