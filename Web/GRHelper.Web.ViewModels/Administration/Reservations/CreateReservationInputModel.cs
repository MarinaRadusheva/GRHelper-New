namespace GRHelper.Web.ViewModels.Administration.Reservations
{
    using System;
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
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        [Required]
        public string Villa { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCount, ErrorMessage = "Please enter a valid guest count.")]
        public int AdultsCount { get; set; }

        public int? ChildrenCount { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
