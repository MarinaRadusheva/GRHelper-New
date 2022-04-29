namespace GRHelper.Services.Data.Tests.Models
{
    using System;

    using GRHelper.Data.Common;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class RequestTestModel : IMapFrom<Request>
    {
        public DateTime Date { get; set; }

        public bool IsDaily { get; set; }

        public int HotelServiceId { get; set; }

        public PaymentType PaymentType { get; set; }

        public TimeSpan Time { get; set; }

        public int ReservationId { get; set; }
    }
}
