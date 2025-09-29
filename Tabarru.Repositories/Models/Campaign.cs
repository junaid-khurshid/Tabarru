using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class Campaign : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey(nameof(Charity))]
        public string CharityId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        // store icon name or url
        [Required]
        public string Icon { get; set; }

        [Required]
        public string ListOfAmounts { get; set; }

        public bool IsEnabled { get; set; } = false;

        public bool IsDefault { get; set; } = false;
    }
}
