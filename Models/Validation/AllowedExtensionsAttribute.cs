using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
namespace BTP.Models.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return ValidationResult.Success; // Si ce n'est pas un fichier, la validation réussit

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_extensions.Contains(fileExtension))
            {
                string allowedExtensions = string.Join(", ", _extensions);
                return new ValidationResult($"Seuls les fichiers avec les extensions suivantes sont autorisés : {allowedExtensions}");
            }

            return ValidationResult.Success; // La validation réussit si l'extension du fichier est autorisée
        }
    }
}
