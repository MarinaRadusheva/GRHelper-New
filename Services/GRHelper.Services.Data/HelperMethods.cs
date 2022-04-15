namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
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

        public static List<string> GetPaymentTypes()
        {
            var paymentTypes = new List<string>();
            foreach (var paymentType in Enum.GetValues(typeof(PaymentType)))
            {
                var payment = paymentType.ToString();
                if (payment != "Free")
                {
                    paymentTypes.Add(payment);
                }
            }

            return paymentTypes;
        }
    }
}
