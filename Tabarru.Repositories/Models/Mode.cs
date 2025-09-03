using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tabarru.Common.Enums;

namespace Tabarru.Repositories.Models
{
    public class Mode : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public Modes ModeType { get; set; }

        [Required]
        public string CampaignId { get; set; }

        [Required]
        public string TemplateId { get; set; }

        public Campaign Campaign { get; set; }
        public Template Template { get; set; }

        [Required]
        public int Amount { get; set; } // User-defined amount
    }
}
