using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tabarru.Repositories.Models
{

    public class PackageDetails
    {
        [Key]
        public int Id { get; set; }  // Hardcoded, not auto-generated

        [Required]
        public string Name { get; set; }

        [Required]
        public string Price { get; set; }

        public string? BillingCycle { get; set; }

        [JsonIgnore]
        public string FeaturesJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Features
        {
            get => string.IsNullOrEmpty(FeaturesJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(FeaturesJson) ?? new List<string>();

            set => FeaturesJson = JsonSerializer.Serialize(value ?? new List<string>());
        }

        public DateTimeOffset CreatedDate { get; set; }
    }
}