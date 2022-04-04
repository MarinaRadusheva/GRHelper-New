namespace GRHelper.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Models;

    public class MainCategory : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.MaxCategoryNameLength)]
        public string Name { get; init; }

        public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
    }
}
