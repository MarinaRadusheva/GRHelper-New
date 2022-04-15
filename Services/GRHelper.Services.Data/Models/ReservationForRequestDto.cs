namespace GRHelper.Services.Data.Models
{
    using System;

    using AutoMapper;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class ReservationForRequestDto : IMapFrom<Reservation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string VillaNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reservation, ReservationForRequestDto>()
                .ForMember(r => r.VillaNumber, opt => opt.MapFrom(v => v.Villa.Number));
        }
    }
}
