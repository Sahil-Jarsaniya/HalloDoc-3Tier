using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class InvoicingVM
    {
        public IEnumerable<Physician> physicians { get; set; }

        public IEnumerable<TimeSheet> pendingTimeSheets { get; set; }
    }

}
