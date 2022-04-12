namespace GRHelper.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GRHelper.Web.ViewModels.Administration.Reservations;

    public interface IReservationsService
    {
        Task CreateAsync(CreateReservationInputModel input);

        int GetCount();

        IEnumerable<T> All<T>();

        Task<T> GetByIdAsync<T>(int id);

        Task EditAsync(EditReservationInputModel input);

        Task DeleteAsync(int id);

        int GetUnlockedCount(string email);

        IEnumerable<T> GetUnlocked<T>(string email);

        IEnumerable<T> AllByGuestId<T>(string id);

        bool Unlock(int id, string password, string userId);

        bool UserIsOwner(int reservationId, string userId);
    }
}
