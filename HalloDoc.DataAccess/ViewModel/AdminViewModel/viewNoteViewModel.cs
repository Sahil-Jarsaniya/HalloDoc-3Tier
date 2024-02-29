using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class viewNoteViewModel
    {
        public int reqClientId { get; set; } 
        public String adminNote { get; set; }
        public IEnumerable<Requestnote> Requestnote { get; set; }
        public IEnumerable<Requeststatuslog> Requeststatuslog { get; set; }
    }
}
