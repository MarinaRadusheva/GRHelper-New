namespace GRHelper.Data.Common.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class NotPastTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            TimeSpan time = ((DateTime)value).TimeOfDay;
            if (time < DateTime.UtcNow.TimeOfDay)
            {
                return false;
            }

            return true;
        }
    }
}
