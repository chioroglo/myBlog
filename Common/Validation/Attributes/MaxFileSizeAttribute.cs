using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.Validation.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeBytes;
        private const double _1KbSizeBytes = 1024;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSizeBytes = maxFileSize;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > _maxFileSizeBytes)
                {
                    return new ValidationResult($"Maximum allowed file size is {_maxFileSizeBytes / _1KbSizeBytes} KB");
                }
            }

            return ValidationResult.Success;
        }
    }
}