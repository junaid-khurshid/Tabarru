using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabarru.Repositories.Models
{
    public class Template : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey(nameof(Charity))]
        public string CharityId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        public ICollection<TemplateCampaign> TemplateCampaigns { get; set; } = new List<TemplateCampaign>();
    }
}