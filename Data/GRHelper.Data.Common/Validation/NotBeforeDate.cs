namespace GRHelper.Data.Common.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class NotBeforeDate : ValidationAttribute
    {
        private string propName;

        public NotBeforeDate(string propertyName)
        {
            this.propName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)validationContext.ObjectInstance.GetType().GetProperty(this.propName).GetValue(validationContext.ObjectInstance);
            var dt = (DateTime)value;
            if (dt <= date)
            {
                return new ValidationResult($"Date must be after {this.propName} date");
            }

            return ValidationResult.Success;
        }
    }
}
