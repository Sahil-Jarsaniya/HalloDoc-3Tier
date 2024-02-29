using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class CancelcaseViewModel
    {
        public int reqClientId { get; set; }

        public string Name { get; set; }
        public String CaseTag { get; set; }

        public String AddCancelationNote { get; set; }
    }
}
