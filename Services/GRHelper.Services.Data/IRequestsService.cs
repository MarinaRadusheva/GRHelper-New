namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GRHelper.Services.Data.Models;
    using GRHelper.Web.ViewModels.Administration.Requests;
    using GRHelper.Web.ViewModels.Guests.Requests;

    public interface IRequestsService
    {
        IEnumerable<T> All<T>();

        IEnumerable<T> All<T>(string datePicker, DateTime startDate, DateTime endDate, List<StatusForRequestSearchModel> statuses, int? reservationNumber);

        IEnumerable<T> AllByReservationId<T>(int id);

        IEnumerable<T> AllByUserId<T>(string userId, string showType);

        IEnumerable<T> GetPending<T>();

        Task<T> GetById<T>(int id);

        bool UserIsOwner(int id, string userId);

        Task CreateAsync(CreateRequestInputModel input, DateTime? endDate);

        Task EditAsync(EditRequestInputModel input, DateTime? endDate);

        Task UpdateStatus(string status, int id);

        Task DeleteAsync(int id);

        CreateRequestInputModel GenerateRequestModel(HotelServiceForRequestDto serviceInfo, List<ReservationForRequestDto> reservations);

        EditRequestInputModel GenerateEditModel(EditRequestInputModel model, HotelServiceForRequestDto serviceInfo);
    }
}
