using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tabarru.Repositories.Models
{
    public class Charity : EntityMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public bool KycStatus { get; set; }

        //Add-Migration Initial""
        //Update-Database
        //Script-Migration -Output script.sql //This generates the SQL script of all pending migrations and saves it to script.sql
        //Get-Migrations // see pending and not migrations
    }
}
