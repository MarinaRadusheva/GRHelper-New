namespace GRHelper.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Models;
    using GRHelper.Data.Common.Validation;

    public class Request : BaseDeletableModel<int>
    {
        public int ReservationId { get; init; }

        public virtual Reservation Reservation { get; init; }

        public int HotelServiceId { get; init; }

        public virtual HotelService HotelService { get; init; }

        public bool IsDaily { get; set; }

        [DataType(DataType.Date)]
        [NotPastDate]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [NotPastTime(nameof(Date))]
        public TimeSpan Time { get; set; }

        [DataType(DataType.Date)]
        [NotPastDate]
        public DateTime? EndDate { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCountPerRequest, ErrorMessage = "Please enter a valid nuber of guests.")]
        public int GuestCount { get; set; }

        public decimal? Price { get; set; }

        public PaymentType PaymentType { get; set; } = PaymentType.Free;

        public RequestStatus RequestStatus { get; set; } = RequestStatus.Waiting;
    }
}
