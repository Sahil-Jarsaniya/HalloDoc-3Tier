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

        public AdminDashboardViewModel searchPatient(searchViewModel obj, AdminDashboardViewModel data);

        public viewCaseViewModel viewCase(int reqClientId);

        public bool viewCase(viewCaseViewModel obj);

        public viewNoteViewModel ViewNoteGet(int reqClientId);

        public void ViewNotePost(int reqClientId, string adminNote, int adminId);

        public void CancelCase(int CaseTag, string addNote, int reqClientId, int adminId);

        public void BlockCase(int reqClientId, string addNote, int adminId);
    }
}
