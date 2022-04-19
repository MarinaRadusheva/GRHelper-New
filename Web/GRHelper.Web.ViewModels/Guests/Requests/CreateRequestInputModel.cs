namespace GRHelper.Web.ViewModels.Guests.Requests
{
    using System.Collections.Generic;

    using GRHelper.Web.ViewModels.Guests.Reservations;

    public class CreateRequestInputModel : BaseRequestInputModel
    {
        public List<ReservationForRequestModel> Reservations { get; set; }
    }
}
