namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using AutoMapper;
    using GRHelper.Data.Common.Validation;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EditReservationInputModel : IMapFrom<Reservation>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [NotPastDate]
        public DateTime From { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [NotPastDate]
        public DateTime To { get; set; }

        public string VillaNumber { get; set; }

        public int AdultsCount { get; set; }

        public int ChildrenCount { get; set; }

        public string Email { get; set; }

        public string GuestId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reservation, EditReservationInputModel>()
                .ForMember(m => m.VillaNumber, opt => opt.MapFrom(r => r.Villa.VillaNumber));
        }
    }
}
