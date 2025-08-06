using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class CharityKycDocuments : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string Logo { get; set; }

        [Required]
        public string UtilityBill { get; set; }

        [Required]
        public string TaxExemptionCertificate { get; set; }

        //public string TaxExemptionCertificate { get; set; }

        [Required]
        public string BankStatement { get; set; }

    }
}
