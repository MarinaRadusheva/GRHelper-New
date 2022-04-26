namespace GRHelper.Data.Common
{
    using System.ComponentModel.DataAnnotations;

    public enum RequestStatus
    {
        [Display(Name = "Not applicable")]
        NA = 0,
        [Display(Name = "Waiting")]
        Waiting = 1,
        [Display(Name = "In progress")]
        InProgress = 2,
        [Display(Name = "Done")]
        Done = 3,
        [Display(Name = "Cancelled")]
        Cancelled = 4,
    }
}
