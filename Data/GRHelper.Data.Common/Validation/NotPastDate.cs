namespace GRHelper.Data.Common.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class NotPastDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = (DateTime)value;
            if (dt < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}
