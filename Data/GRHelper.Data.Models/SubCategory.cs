namespace GRHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Models;

    public class SubCategory : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.MaxCategoryNameLength)]
        public string Name { get; init; }

        public int MainCategoryId { get; init; }

        public virtual MainCategory MainCategory { get; init; }

        public virtual ICollection<HotelService> HotelServices { get; set; } = new HashSet<HotelService>();
    }
}
