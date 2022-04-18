namespace GRHelper.Web.ViewComponents
{
    using System;
    using GRHelper.Data.Common;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.AspNetCore.Mvc;

    public class ManageRequestButtonsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int requestId, string status, DateTime startDate, DateTime? endDate)
        {
            bool canBeCancelled = true;
            bool canBeDeleted = true;
            bool canBeEdited = true;
            if (status != RequestStatus.Waiting.ToString())
            {
                canBeEdited = false;
                canBeDeleted = false;
            }

            if (status == RequestStatus.Done.ToString() || status == RequestStatus.Cancelled.ToString())
            {
                canBeCancelled = false;
            }

            if ((endDate != null && endDate < DateTime.UtcNow) || (endDate == null && startDate < DateTime.UtcNow))
            {
                canBeEdited = false;
                canBeCancelled = false;
                canBeDeleted = false;
            }

            var buttonsModel = new ManageButtonsViewModel()
            {
                Id = requestId,
                CanEdit = canBeEdited,
                CanCancel = canBeCancelled,
                CanDelete = canBeDeleted,
            };

            return this.View(buttonsModel);
        }
    }
}
