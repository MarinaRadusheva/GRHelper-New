using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRHelper.Web.ViewModels.Administration.Requests
{
    public class AllRequestsViewModel
    {
        public IEnumerable<RequestListViewModel> Requests { get; set; }

        public string DatesTerm { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
