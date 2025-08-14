using System.ComponentModel.DataAnnotations;

namespace Tabarru.Attributes
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        private readonly List<string> contentTypes;
        private readonly int? minAcceptedSize;
        private readonly int? maxAcceptedSize;

        /// <summary>
        /// min and max accept size will use from configuration and default accepted types ("image/png", "image/jpeg", "image/jpg", "application/pdf") 
        /// </summary>
        public ValidateFileAttribute()
        {
            contentTypes = new List<string> { "image/png", "image/jpeg", "image/jpg", "application/pdf" };
        }

        /// <summary>
        /// min and max accept size will use from configuration
        /// </summary>
        /// <param name="acceptedTypes"></param>
        public ValidateFileAttribute(List<string> acceptedTypes)
        {
            contentTypes = acceptedTypes;
        }

        public ValidateFileAttribute(int minimumSizeInKb, int maximumSizeInKb)
        {
            contentTypes = new List<string> { "image/png", "image/jpeg", "image/jpg", "application/pdf" }; ;
            minAcceptedSize = minimumSizeInKb;
            maxAcceptedSize = maximumSizeInKb;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IConfiguration configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
            IFormFile formFile = (IFormFile)value;
            if (formFile == null)
                return ValidationResult.Success;

            int minimumSizeInKb = (minAcceptedSize ?? configuration.GetValue<int>("ImageSize:MinimumSizeInKb")) * 1024;
            int maximumSizeInKb = (maxAcceptedSize ?? configuration.GetValue<int>("ImageSize:MaximumSizeInKb")) * 1024;

            if (!contentTypes.Any(x => x.Equals(formFile.ContentType)))
                return new ValidationResult("INVALID_FILE_FORMAT");

            else if (formFile.Length < minimumSizeInKb)
                return new ValidationResult("SIZE_TOO_SMALL");

            else if (formFile.Length > maximumSizeInKb)
                return new ValidationResult("SIZE_TOO_LARGE");

            return ValidationResult.Success;
        }
    }
}
