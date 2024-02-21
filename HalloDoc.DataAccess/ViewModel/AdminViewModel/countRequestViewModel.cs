using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class countRequestViewModel
    {
        public int newCount { get; set; }

        public int pendingCount { get; set; }

        public int activeCount { get; set; }

        public int concludeCount { get; set; }
        public int closeCount { get; set; }
        public int unpaidCount { get; set; }
    }
}
