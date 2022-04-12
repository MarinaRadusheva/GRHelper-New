namespace GRHelper.Web.ViewModels.Guests.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class RequestListViewModel : IMapFrom<Request>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public string HotelServiceName { get; set; }

        public bool IsDaily { get; set; }

        public DateTime? Date { get; set; }

        public DateTime Time { get; set; }

        public int GuestCount { get; set; }

        public string RequestStatus { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Request, RequestListViewModel>()
                .ForMember(r => r.ReservationNumber, opt => opt.MapFrom(r => r.Reservation.ReservationNumber));
        }
    }
}
