using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class Device : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string DeviceName { get; set; } = string.Empty;

        [Required]
        public string CharityId { get; set; }

        [Required]
        public string TemplateId { get; set; }

        [Required]
        public string DeviceLocation { get; set; } = string.Empty;

        [ForeignKey(nameof(TemplateId))]
        public Template Template { get; set; }
    }
}
