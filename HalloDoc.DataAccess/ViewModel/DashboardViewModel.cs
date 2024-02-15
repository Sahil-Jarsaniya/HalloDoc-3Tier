using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class DashboardViewModel
    {
        public ProfileEditViewModel ProfileEditViewModel { get; set; }

        public IEnumerable<PatientDashboardViewModel> PatientDashboardViewModel { get; set; }
    }
}
