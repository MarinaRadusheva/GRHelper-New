namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System;
    using System.Linq;

    using AutoMapper;

    using GRHelper.Data.Common;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class AllReservationsViewModel : IMapFrom<Reservation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public DateTime From { get; set; }

        public string Name { get; set; }

        public string VillaNumber { get; set; }

        public int RequestsCount { get; set; }

        public int PendingRequestsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reservation, AllReservationsViewModel>()
                .ForMember(m => m.VillaNumber, opt => opt.MapFrom(r => r.Villa.VillaNumber))
                .ForMember(m => m.RequestsCount, opt => opt.MapFrom(r => r.Requests.Count))
                .ForMember(m => m.PendingRequestsCount, opt => opt.MapFrom(r => r.Requests
                        .Where(x => x.RequestStatus == RequestStatus.Waiting || x.RequestStatus == RequestStatus.InProgress)
                        .ToList().Count));
        }
    }
}
