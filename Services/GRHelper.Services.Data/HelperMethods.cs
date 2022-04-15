namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GRHelper.Data.Common;

    public class HelperMethods
    {
        private readonly IReservationsService reservationsService;

        public HelperMethods(IReservationsService reservationsService)
        {
            this.reservationsService = reservationsService;
        }

        public static string GeneratePassword(int length)
        {
            {
                const string valid = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ123456789";
                StringBuilder res = new();
                Random rnd = new();
                while (length-- > 0)
                {
                    res.Append(valid[rnd.Next(valid.Length - 1)]);
                }

                return res.ToString();
            }
        }

        public static IEnumerable<PaymentType> GetPaymentTypes()
        {

            return (PaymentType[])Enum.GetValues(typeof(PaymentType));
        }

        public static TAttribute GetAttribute<TAttribute>(Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
