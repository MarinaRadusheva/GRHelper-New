namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using GRHelper.Data.Common;
    using GRHelper.Web.ViewModels.Administration.Requests;
    using GRHelper.Web.ViewModels.Guests.Requests;

    public class HelperMethods
    {
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

        public static IEnumerable<T> GetEnumTypes<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static TAttribute GetAttribute<TAttribute>(Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static List<StatusForRequestSearchModel> LoadStatuses()
        {
            var allStatuses = new List<StatusForRequestSearchModel>();
            var enumValues = GetEnumTypes<RequestStatus>();
            foreach (var enumValue in enumValues)
            {
                if (enumValue != 0)
                {
                    allStatuses.Add(new StatusForRequestSearchModel
                    {
                        DisplayName = HelperMethods.GetAttribute<DisplayAttribute>(enumValue).Name,
                        EnumString = enumValue.ToString(),
                        EnumValue = (int)enumValue,
                        Selected = false,
                    });
                }
            }

            return allStatuses;
        }

        public static bool RequestDateIsValid(ReservationDatesModel reservationDates, DateTime date)
        {
            if (date < reservationDates.From.Date || date > reservationDates.To.Date)
            {
                return false;
            }

            return true;
        }
    }
}
