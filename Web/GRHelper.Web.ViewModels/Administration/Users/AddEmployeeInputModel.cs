namespace GRHelper.Web.ViewModels.Administration.Users
{
    using System.ComponentModel.DataAnnotations;

    public class AddEmployeeInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
