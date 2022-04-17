namespace GRHelper.Data.Common.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class NotPastTime : ValidationAttribute
    {
        private string propName;

        public NotPastTime(string propertyName)
        {
            this.propName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)validationContext.ObjectInstance.GetType().GetProperty(this.propName).GetValue(validationContext.ObjectInstance);
            TimeSpan time = (TimeSpan)value;
            if (date == DateTime.Today && time < DateTime.UtcNow.TimeOfDay)
            {
                return new ValidationResult($"Please enter time in the future");
            }

            return ValidationResult.Success;
        }
    }
}
