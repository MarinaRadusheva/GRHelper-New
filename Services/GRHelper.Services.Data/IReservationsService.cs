namespace GRHelper.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GRHelper.Services.Data.Models;
    using GRHelper.Web.ViewModels.Administration.Reservations;

    public interface IReservationsService
    {
        Task<int> CreateAsync(CreateReservationInputModel input);

        int GetCount(bool active);

        IEnumerable<T> All<T>(bool active, int pageNumber);

        IEnumerable<T> GetBySearchTerms<T>(int? number);

        Task<T> GetByIdAsync<T>(int id);

        Task EditAsync(EditReservationInputModel input);

        Task DeleteAsync(int id);

        int GetUnlockedCount(string email);

        IEnumerable<T> GetUnlocked<T>(string email);

        IEnumerable<T> AllByGuestId<T>(string id);

        List<ReservationForRequestDto> AvailableByGuestId(string id);

        bool Unlock(int id, string password, string userId);

        bool UserIsOwner(int reservationId, string userId);
        Task<bool> SendPassword(int id);
    }
}
