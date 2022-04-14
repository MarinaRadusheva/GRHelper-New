namespace GRHelper.Services.Data
{
    using System;
    using System.Text;

    using GRHelper.Web.ViewModels.Guests.Requests;

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

        public CreateRequestInputModel GenerateRequestModel(int serviceId, string userId)
        {
            var reservations = this.reservationsService.AvailableByGuestId(userId);
            return new CreateRequestInputModel();
        }
    }
}
