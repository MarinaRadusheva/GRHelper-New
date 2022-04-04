namespace GRHelper.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Models;

    public class HotelService : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.MaxServiceNameLength)]
        public string Name { get; init; }

        public decimal? Price { get; set; }

        public int SubCategoryId { get; init; }

        public virtual SubCategory SubCategory { get; init; }
    }
}
