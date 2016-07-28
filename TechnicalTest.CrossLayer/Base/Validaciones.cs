
using System;
using System.ComponentModel.DataAnnotations;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Class NombreUnico
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class NombreUnico : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    ValidationResult validationResult = new ValidationResult(ErrorMessageString);
        //    return validationResult;
        //}

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            return false;
        }
    }
}