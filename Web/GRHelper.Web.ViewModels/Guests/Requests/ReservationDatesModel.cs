namespace GRHelpe.Web.ViewModels.Guests.Requests
{
    using System;

    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class ReservationDatesModel : IMapFrom<Reservation>
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
