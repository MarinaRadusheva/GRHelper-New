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
        [NotBeforeDate(nameof(From))]
        public DateTime To { get; set; }

        public int ReservationNumber { get; set; }

        [Required]
        public string Name { get; set; }

        public int VillaId { get; set; }

        public virtual Villa Villa { get; set; }

        [Range(DataConstants.MinGuestCount, DataConstants.MaxGuestCount, ErrorMessage = "Please enter a valid guest count.")]
        public int AdultsCount { get; set; }

        [Range(DataConstants.MinChildrenCount, DataConstants.MaxChildrenCount, ErrorMessage = "Please enter a valid children count")]
        public int ChildrenCount { get; set; } = DataConstants.MinChildrenCount;

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; init; }

        public bool Unlocked { get; set; } = false;

        public string GuestId { get; set; }

        public virtual ApplicationUser Guest { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
