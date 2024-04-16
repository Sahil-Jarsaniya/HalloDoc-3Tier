using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class UserAccessVM
    {
        public IEnumerable<Role>? Roles { get; set; }    

        public IQueryable<UserAccessTable>? userAccessTables { get; set; }
    }

    public class UserAccessTable
    {
        public int? AccountTypeId { get; set; }
        public string? AccountType { get; set; }
        public string? UserName { get; set;}
        public string? Phone { get; set;}
        public string? status { get; set;}
        public int openReq { get; set;}
        public int? userId { get; set;}
    }
}
