using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRHelper.Web.ViewModels.Administration.Requests
{
    public class AllRequestsViewModel
    {
        public IEnumerable<RequestListViewModel> Requests { get; set; }

        public string DatePicker { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime To { get; set; }
    }
}
