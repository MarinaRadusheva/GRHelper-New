namespace GRHelper.Web.ViewModels.Guests.Reservations
{
    using System;

    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class UnlockedReservationViewModel : IMapFrom<Reservation>
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
