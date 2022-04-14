namespace GRHelper.Services.Data.Models
{
    using System;

    using AutoMapper;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class ReservationForRequestDto : IMapFrom<Reservation>
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string VillaNumber { get; set; }

        public override string ToString()
        {
            return this.ReservationNumber + " - " + this.VillaNumber;
        }
    }
}
