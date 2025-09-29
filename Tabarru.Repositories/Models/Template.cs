using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class Template : EntityMetaData
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(Charity))]
        public string CharityId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public string Icon { get; set; }
        public string Message { get; set; }

        public ICollection<Mode> Modes { get; set; } = new List<Mode>();
    }
}