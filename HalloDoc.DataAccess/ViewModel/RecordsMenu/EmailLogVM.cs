using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.RecordsMenu
{
    public class EmailLogVM
    {
        public int id { get; set; }
        public string? Recipient { get; set; }

        public string? Action { get; set; }
        public int? RoleId { get; set; }
        public string? Rolename { get; set; }
        public string? EmailId { get; set; }
        public string? PhoneNumber{ get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime SentDate { get; set; }
        public bool? sent { get; set; }
        public int? sentTries { get; set; }
        public string ConfirmationNumber { get; set; }

    }
}
