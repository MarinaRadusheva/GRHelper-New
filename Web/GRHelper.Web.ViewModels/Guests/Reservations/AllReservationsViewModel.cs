namespace GRHelper.Web.ViewModels.Guests.Reservations
{
    using System.Collections.Generic;

    public class AllReservationsViewModel
    {
        public ICollection<ReservationListViewModel> PresentReservations { get; set; } = new List<ReservationListViewModel>();

        public ICollection<ReservationListViewModel> FutureReservations { get; set; } = new List<ReservationListViewModel>();

        public ICollection<ReservationListViewModel> PastReservations { get; set; } = new List<ReservationListViewModel>();

        public bool UnlockedExist { get; set; }
    }
}
