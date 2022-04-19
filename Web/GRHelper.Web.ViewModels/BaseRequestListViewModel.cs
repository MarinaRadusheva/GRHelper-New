namespace GRHelper.Web.ViewModels
{
    using System;

    public abstract class BaseRequestListViewModel
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public string HotelServiceName { get; set; }

        public bool IsDaily { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public DateTime? EndDate { get; set; }

        public int GuestCount { get; set; }

        public decimal? Price { get; set; }

        public string RequestStatus { get; set; }
    }
}
