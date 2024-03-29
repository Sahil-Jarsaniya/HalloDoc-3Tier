using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.ProvidersMenu
{
    public class Scheduling
    {
        public DateOnly Date { get; set; }

        public IEnumerable<Region> Regions { get; set; }
    }
}
