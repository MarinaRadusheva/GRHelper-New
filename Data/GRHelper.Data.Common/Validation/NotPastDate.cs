namespace GRHelper.Data.Common.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class NotPastDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = (DateTime)value;
            if (dt.Date < DateTime.UtcNow.Date)
            {
                return false;
            }

            return true;
        }
    }
}
