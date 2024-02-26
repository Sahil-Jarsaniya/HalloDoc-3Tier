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



    }
}
