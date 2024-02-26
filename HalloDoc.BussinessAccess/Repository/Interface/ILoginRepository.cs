using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface ILoginRepository
    {
        public string GetHash(string text);

        public Admin GetLoginData(AspNetUser obj, String hashPass);

        public User PatientLogin(AspNetUser obj, String hashPass);

        public void SendEmail(String email);
    }
}
