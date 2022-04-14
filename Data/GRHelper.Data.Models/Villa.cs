namespace GRHelper.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Data.Common.Models;

    public class Villa : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DataConstants.MaxVillaNumberLength)]
        public string Number { get; init; }
    }
}
