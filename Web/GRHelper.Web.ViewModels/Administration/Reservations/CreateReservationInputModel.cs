namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Validation;

    public class CreateReservationInputModel
    {
        [Required]
        [NotPastDate]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [Required]
        [NotPastDate]
        [NotBeforeDate(nameof(From))]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        public int Number { get; set; }

        public IEnumerable<string> Villas { get; set; }

        [Required]
        public string Villa { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCount, ErrorMessage = "Please enter a valid guest count.")]
        public int AdultsCount { get; set; }

        [Range(DataConstants.MinChildrenCount, DataConstants.MaxChildrenCount, ErrorMessage = "Please enter a valid children count")]
        public int ChildrenCount { get; set; }

        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
