using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IRequestRepository
    {
        public string GetConfirmationNumber(DateTime createtime, String lastName, string firstName);

        public List<DataAccess.Models.Region> Regions();
        public void CreatePatientRequest(PatientViewModel obj);

        public void CreateFamilyfriendRequest(FamilyViewModel obj);

        public void CreateConciergeRequest(ConciergeViewModel obj);

        public void CreateBusinessRequest(BussinessViewModel obj);
        public void CreateRequestByPhysician(PatientViewModel obj, int PhyId);

        public bool emailExist(string email);
    }
}
