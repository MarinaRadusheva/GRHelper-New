namespace GRHelper.Web.ViewComponents
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using GRHelper.Data.Common;
    using GRHelper.Services.Data;
    using GRHelper.Web.ViewModels.Guests.Requests;
    using Microsoft.AspNetCore.Mvc;

    public class ManageRequestButtonsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(bool isEmployee, int requestId, string status, DateTime startDate, DateTime? endDate)
        {
            bool canBeCancelled = true;
            bool canBeDeleted = true;
            bool canBeEdited = true;
            bool canBeChanged = false;
            string changeBtnText = string.Empty;
            bool pastRequest = (endDate != null && endDate < DateTime.UtcNow.Date) || (endDate == null && startDate < DateTime.UtcNow.Date);
            bool futureRequest = startDate > DateTime.UtcNow.Date;
            if (status != RequestStatus.Waiting.ToString())
            {
                canBeEdited = false;
                canBeDeleted = false;
            }

            if (status == RequestStatus.Done.ToString() || status == RequestStatus.Cancelled.ToString())
            {
                canBeCancelled = false;
            }

            if (pastRequest)
            {
                canBeEdited = false;
                canBeCancelled = false;
                canBeDeleted = false;
            }

            if (isEmployee)
            {
                canBeDeleted = false;
                if (status == RequestStatus.Waiting.ToString() && !pastRequest && !futureRequest)
                {
                    canBeChanged = true;
                    changeBtnText = HelperMethods.GetAttribute<DisplayAttribute>(RequestStatus.InProgress).Name;
                }
                else if (status == RequestStatus.InProgress.ToString() && !pastRequest && !futureRequest)
                {
                    canBeChanged = true;
                    changeBtnText = HelperMethods.GetAttribute<DisplayAttribute>(RequestStatus.Done).Name;
                }
                else
                {
                    canBeChanged = false;
                    changeBtnText = status;
                }
            }

            var buttonsModel = new ManageButtonsViewModel()
            {
                Id = requestId,
                IsEmployee = isEmployee,
                CanEdit = canBeEdited,
                CanCancel = canBeCancelled,
                CanDelete = canBeDeleted,
                CanChange = canBeChanged,
                ChangeBtnText = changeBtnText,
            };

            return this.View(buttonsModel);
        }
    }
}
