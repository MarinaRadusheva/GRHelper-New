namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Validation;
    using GRHelper.Data.Models;
    using GRHelper.Services.Mapping;

    public class EditReservationInputModel : IMapFrom<Reservation>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [Required]
        [NotBeforeDate(nameof(From))]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        public IEnumerable<string> Villas { get; set; }

        public string VillaNumber { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCount, ErrorMessage = "Please enter a valid guest count.")]
        public int AdultsCount { get; set; }

        [Range(DataConstants.MinChildrenCount, DataConstants.MaxChildrenCount, ErrorMessage = "Please enter a valid children count")]
        public int ChildrenCount { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
