using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class PasswordResetToken : EntityMetaDataWithDeleteAble
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string CharityId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; } = false;

        [ForeignKey(nameof(CharityId))]
        public Charity Charity { get; set; }
    }
}
