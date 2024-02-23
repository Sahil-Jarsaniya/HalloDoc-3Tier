using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IAdminDashboardRepository
    {
        public AdminDashboardViewModel adminDashboard();

        public viewCaseViewModel viewCase(int reqClientId);

        public bool viewCase(viewCaseViewModel obj);
    }
}
