using System.ComponentModel.DataAnnotations;

namespace Tabarru.Repositories.Models
{
    public class TemplateCampaign
    {
        [Key]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [Key]
        public string CampaignId { get; set; }
        public Campaign Campaign { get; set; }
    }
}
