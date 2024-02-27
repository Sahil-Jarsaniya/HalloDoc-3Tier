using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class AdminDashboardViewModel
    {
        public  countRequestViewModel countRequestViewModel { get; set; }

        public string? Name { get; set; }
        public string? sorting { get; set; }

        public IEnumerable<newReqViewModel> newReqViewModel { get; set; }

        public IEnumerable<pendingReqViewModel> pendingReqViewModel { get; set; }

        public IEnumerable<concludeReqViewModel> concludeReqViewModel { get; set; }

        public IEnumerable<closeReqViewModel> closeReqViewModels { get; set; }

        public IEnumerable<unpaidReqViewModel> unpaidReqViewModels { get; set; }
        public IEnumerable<activeReqViewModel> activeReqViewModels { get; set; }
    }
}