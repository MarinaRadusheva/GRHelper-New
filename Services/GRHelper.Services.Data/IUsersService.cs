namespace GRHelper.Services.Data
{
    using System.Threading.Tasks;

    using GRHelper.Web.ViewModels.Administration.Users;

    public interface IUsersService
    {
        Task DeleteUserAsync(string email);

        Task AddEmployeeAsync(AddEmployeeInputModel model);
    }
}
