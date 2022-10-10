using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.Validation.Attributes
{
    public class MaxAvatarFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeBytes;
        private const double _1MbSizeBytes = 1048576.0;

        public MaxAvatarFileSizeAttribute(int maxFileSize)
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
                    return new ValidationResult($"Maximum allowed file size is {_maxFileSizeBytes / _1MbSizeBytes} MB");
                }
            }
            return ValidationResult.Success;
        }
    }
}
