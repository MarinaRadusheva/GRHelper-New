namespace GRHelper.Web.ViewComponents
{
    using System;
    
    using GRHelper.Web.ViewModels.Administration.Reservations;
    using Microsoft.AspNetCore.Mvc;

    public class ManageResButtonsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int id, int requestsCount, DateTime dateTo)
        {
            var model = new ManageResButtonsViewModel()
            {
                ReservationId = id,
                CanEdit = dateTo.Date >= DateTime.UtcNow.Date,
                CanDelete = requestsCount == 0,
            };
            return this.View(model);
        }
    }
}
