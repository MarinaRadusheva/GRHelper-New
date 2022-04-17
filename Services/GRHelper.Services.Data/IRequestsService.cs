namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GRHelper.Services.Data.Models;
    using GRHelper.Web.ViewModels.Guests.Requests;

    public interface IRequestsService
    {
        IEnumerable<T> AllByReservationId<T>(int id);

        IEnumerable<T> AllByUserId<T>(string userId);

        Task<T> GetById<T>(int id);

        Task CreateAsync(CreateRequestInputModel input, DateTime? endDate);

        Task EditAsync(EditRequestInputModel input);

        Task UpdateStatus(string status, int id);

        Task DeleteAsync(int id);

        CreateRequestInputModel GenerateRequestModel(HotelServiceForRequestDto serviceInfo, List<ReservationForRequestDto> reservations);
    }
}
