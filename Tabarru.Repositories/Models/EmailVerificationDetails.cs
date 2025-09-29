using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class EmailVerificationDetails : EntityMetaData
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey(nameof(Charity))]
        public string CharityId { get; set; }

        public string Token { get; set; } // 6-digit code as string
        public string Email { get; set; }

        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
