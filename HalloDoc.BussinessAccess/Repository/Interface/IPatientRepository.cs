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

        public String PatientProfile(ProfileEditViewModel obj);

        public String CreateReqMeOrElse(PatientViewModel obj, string aspId);

        public AgreementViewModel ReviewAgreement(String reqClientId);

        public void Agree(int reqClientId);
        public void DisAgree(AgreementViewModel obj);

 

    }
}
