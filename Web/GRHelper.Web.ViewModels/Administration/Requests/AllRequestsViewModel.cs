namespace GRHelper.Web.ViewModels.Administration.Requests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllRequestsViewModel
    {
        public IEnumerable<RequestListViewModel> Requests { get; set; }

        public string DatePicker { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        public List<StatusForRequestSearchModel> Statuses { get; set; }
    }
}
