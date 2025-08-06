using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tabarru.Common.Enums;


namespace Tabarru.Repositories.Models
{
    public class CharityKycDetails : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey(nameof(Charity))]
        public string CharityId { get; set; }

        [Required]
        public CharityKycStatus Status { get; set; }

        [Required]
        public bool IsCharityDocumentUploaded { get; set; }

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        public string CharityName { get; set; }

        [Required]
        [MaxLength(200)]
        public string CountryCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string CharityNumber { get; set; }


        [ForeignKey(nameof(CharityKycDocuments))]
        public string CharityKycDocumentsId { get; set; }


        public virtual CharityKycDocuments CharityKycDocuments { get; set; }
    }
}
