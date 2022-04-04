namespace GRHelper.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Models;
    using GRHelper.Data.Common.Validation;

    public class Reservation : BaseDeletableModel<int>
    {
        [Required]
        [NotPastDate]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public int VillaId { get; set; }

        [Required]
        public virtual Villa Villa { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCount, ErrorMessage = "Please enter a valid guest count.")]
        public int AdultsCount { get; set; }

        public int? ChildrenCount { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; init; } = HelperMethods.GeneratePassword(DataConstants.ResPasswordLength);

        public ApplicationUser Guest { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
