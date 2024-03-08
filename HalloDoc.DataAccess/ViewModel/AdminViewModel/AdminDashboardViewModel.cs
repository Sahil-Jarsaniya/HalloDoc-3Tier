using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class AdminDashboardViewModel
    {
        public int? status { get; set; }
        public  countRequestViewModel countRequestViewModel { get; set; }

        public IEnumerable<Casetag> Casetag { get; set; }

        public IEnumerable<Region> Region { get; set; }

        public string? Name { get; set; }
        public string? sorting { get; set; }

        public IEnumerable<newReqViewModel> newReqViewModel { get; set; }

        public IEnumerable<pendingReqViewModel> pendingReqViewModel { get; set; }

        public IEnumerable<concludeReqViewModel> concludeReqViewModel { get; set; }

        public IEnumerable<closeReqViewModel> closeReqViewModels { get; set; }

        public IEnumerable<unpaidReqViewModel> unpaidReqViewModels { get; set; }
        public IEnumerable<activeReqViewModel> activeReqViewModels { get; set; }

        public CancelcaseViewModel CancelcaseViewModel { get; set; }
    }
}