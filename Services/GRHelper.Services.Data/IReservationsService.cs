namespace GRHelper.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GRHelper.Web.ViewModels.Administration.Reservations;

    public interface IReservationsService
    {
        Task CreateAsync(CreateReservationInputModel input);

        int GetCount();
    }
}
