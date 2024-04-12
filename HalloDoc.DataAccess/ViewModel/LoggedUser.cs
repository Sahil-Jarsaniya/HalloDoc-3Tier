using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class LoggedUser
    {
        public required string AspId { get; set;}
        public required string FirstName { get; set;}
        public  string? LastName { get; set;}
        public required string Email { get; set;}
        public string? Role { get; set;}
        public string? Roleid { get; set;}
    }
}
