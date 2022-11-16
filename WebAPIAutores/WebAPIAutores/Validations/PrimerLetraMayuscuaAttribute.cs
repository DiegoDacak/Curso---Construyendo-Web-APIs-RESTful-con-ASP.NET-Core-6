using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.Validations
{
    public class PrimerLetraMayuscuaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primerLetra = value.ToString()![0].ToString();
            if (primerLetra != primerLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayuscula");
            }
            return ValidationResult.Success;
        }
    }
}