using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class CharityKycDocuments : EntityMetaDataWithDeleteAble
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey(nameof(CharityKycDetails))]
        public string CharityKycDetailsId { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required]
        public string IncorporationCertificate { get; set; }

        public string UtilityBill { get; set; }

        public string TaxExemptionCertificate { get; set; }

        public string BankStatement { get; set; }
    }
}
