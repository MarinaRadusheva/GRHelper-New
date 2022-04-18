namespace GRHelper.Web.ViewModels.Guests.Requests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Validation;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class EditRequestInputModel : IMapFrom<Request>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ReservationId { get; set; }

        public int HotelServiceId { get; init; }

        public bool IsDaily { get; set; }

        [DataType(DataType.Date)]
        [NotPastDate]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [NotPastTime(nameof(Date))]
        public TimeSpan Time { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCountPerRequest, ErrorMessage = "Please enter a valid nuber of guests.")]
        public int GuestCount { get; set; }

        public IEnumerable<PaymentTypeForRequest> PaymentTypes { get; set; } = new List<PaymentTypeForRequest>();

        public string PaymentType { get; set; } = Data.Common.PaymentType.Free.ToString();
    }
}
