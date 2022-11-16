using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MoviesApi.Common.Messages;

namespace MoviesApi.Validations
{
    public class ArchiveSizeValidation : ValidationAttribute
    {
        private readonly int _maxSizeMb;

        public ArchiveSizeValidation(int maxSizeMb)
        {
            _maxSizeMb = maxSizeMb;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile formFile)
            {
                return ValidationResult.Success;
            }

            return formFile.Length > _maxSizeMb * 1024 * 1024 ? 
                new ValidationResult(ValidationMessages.MaxSize(_maxSizeMb)) : 
                ValidationResult.Success;
        }
    }
}