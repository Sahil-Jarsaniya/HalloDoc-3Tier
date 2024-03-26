using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public  class CreateRole
    {
        public  int Roleid { get; set; }
        public string? Name { get; set; }
        public string? AccountType { get; set; }

        public bool? isdeleted { get; set; }
        public IEnumerable<Menu>? Menu { get; set; }
        //public IEnumerable<Menu>? SelectedMenu { get; set; }

        public IEnumerable<AccountType>? accountTypes { get; set; }

        public IEnumerable<CheckBoxData>? SelectedPage { get; set; }
    }
}
