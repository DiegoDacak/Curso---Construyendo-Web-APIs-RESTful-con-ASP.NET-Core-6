using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MoviesApi.Common.Enum;
using MoviesApi.Common.Messages;

namespace MoviesApi.Validations
{
    public class ArchiveTypeValidation : ValidationAttribute
    {
        private readonly List<string> _validTypes;

        public ArchiveTypeValidation(List<string> validTypes)
        {
            _validTypes = validTypes;
        }

        public ArchiveTypeValidation(ArchiveTypeGroup archiveTypeGroup)
        {
            if (archiveTypeGroup == ArchiveTypeGroup.Image)
            {
                _validTypes = new List<string>() { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile formFile)
            {
                return ValidationResult.Success;
            }

            return !_validTypes.Contains(formFile.ContentType) ? 
                new ValidationResult(ValidationMessages.ArchiveValidTypes(_validTypes)) : 
                ValidationResult.Success;
        }
    }
}