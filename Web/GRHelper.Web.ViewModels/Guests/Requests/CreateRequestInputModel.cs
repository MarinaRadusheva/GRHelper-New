namespace GRHelper.Web.ViewModels.Guests.Requests
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Validation;

    public class CreateRequestInputModel
    {
        public int ReservationId { get; init; }

        public int HotelServiceId { get; init; }

        public bool IsDaily { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [DataType(DataType.Time)]
        [NotPastTime]
        public DateTime Time { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCountPerRequest, ErrorMessage = "Please enter a valid nuber of guests.")]
        public int GuestCount { get; set; }

        public PaymentType PaymentType { get; set; } = PaymentType.Free;
    }
}
