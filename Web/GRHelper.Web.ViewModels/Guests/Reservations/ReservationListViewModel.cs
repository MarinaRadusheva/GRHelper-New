namespace GRHelper.Web.ViewModels.Guests.Reservations
{
    using System;

    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class ReservationListViewModel : IMapFrom<Reservation>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
