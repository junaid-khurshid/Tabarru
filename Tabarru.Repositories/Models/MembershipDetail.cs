using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class MembershipDetail : EntityMetaDataWithDeleteAble
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string PaymentDetailId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HouseNumberAndName { get; set; }
        public string PostalCode { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
    }
}
