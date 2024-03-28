using HalloDoc.DataAccess.Models;
using HalloDoc.Services;
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

        public PaginatedList<newReqViewModel> newReqViewModel { get; set; }

        public PaginatedList<pendingReqViewModel> pendingReqViewModel { get; set; }

        public PaginatedList<concludeReqViewModel> concludeReqViewModel { get; set; }

        public PaginatedList<closeReqViewModel> closeReqViewModels { get; set; }

        public PaginatedList<unpaidReqViewModel> unpaidReqViewModels { get; set; }
        public PaginatedList<activeReqViewModel> activeReqViewModels { get; set; }

        public CancelcaseViewModel CancelcaseViewModel { get; set; }
    }
}