namespace GRHelper.Data.Common
{
    using System.ComponentModel.DataAnnotations;

    public enum RequestStatus
    {
        NA = 0,
        Waiting = 1,
        [Display(Name = "In progress")]
        InProgress = 2,
        Done = 3,
        Cancelled = 4,
    }
}
