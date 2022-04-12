namespace GRHelper.Services.Data
{
    using GRHelper.Web.ViewModels.Guests.Requests;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRequestsService
    {
        IEnumerable<T> AllByReservationId<T>(int id);

        IEnumerable<T> AllByUserId<T>(string userId);

        Task<T> GetById<T>(int id);

        Task CreateAsync(CreateRequestInputModel input);

        Task EditAsync(EditRequestInputModel input);

        Task UpdateStatus(string status, int id);

        Task DeleteAsync(int id);
    }
}
