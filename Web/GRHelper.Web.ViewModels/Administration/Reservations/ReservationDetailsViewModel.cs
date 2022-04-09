namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System;

    using AutoMapper;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class ReservationDetailsViewModel : IMapFrom<Reservation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string VillaNumber { get; set; }

        public int AdultsCount { get; set; }

        public int ChildrenCount { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reservation, ReservationDetailsViewModel>()
                .ForMember(m => m.VillaNumber, opt => opt.MapFrom(r => r.Villa.VillaNumber));
        }
    }
}
