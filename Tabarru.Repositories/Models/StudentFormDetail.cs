using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class StudentFormDetail : EntityMetaDataWithDeleteAble
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string PaymentDetailId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public string StudentName { get; set; }
        public string ParentName { get; set; }
        public string FullAddress { get; set; }
        public string StudentId { get; set; }
        public string ParentId { get; set; }
        public int Amount { get; set; }
        public string Period { get; set; }
        public string Notes { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
    }
}
