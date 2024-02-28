using HalloDoc.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IPatientRepository
    {

        public DashboardViewModel PatientDashboard(String AspId);

        public DocumentViewModel Document(int reqId);

        public int Document(UploadFileViewModel obj);

        public String PatientProfile(DashboardViewModel obj);

        public String CreateReqMeOrElse(PatientViewModel obj, int uid);
    }
}
