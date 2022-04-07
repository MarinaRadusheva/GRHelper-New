using AutoMapper;
using GRHelper.Data.Models;
using GRHelper.Services.Mapping;
using System;


namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    public class ReservationDetailsViewModel : IMapFrom<Reservation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string VillaNumber { get; set; }

        public int AdultsCount { get; set; }

        public int? ChildrenCount { get; set; }

        public string Email { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reservation, ReservationDetailsViewModel>()
                .ForMember(m => m.VillaNumber, opt => opt.MapFrom(r => r.Villa.VillaNumber));
        }
    }
}
