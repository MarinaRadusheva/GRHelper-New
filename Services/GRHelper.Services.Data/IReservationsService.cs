﻿namespace GRHelper.Services.Data
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
    }
}
