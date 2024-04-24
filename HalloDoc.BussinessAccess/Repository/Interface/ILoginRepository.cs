using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface ILoginRepository
    {
        public bool isEmailAvailable(string email);
        public string GetHash(string text);

        public AspNetUser GetLoginData(login obj, String hashPass);

        public void SendEmail(String email, string subject, string body);

        public void uploadFile(IFormFile? fileName,string folder, string path);

        public Admin isAdmin(string AspId);
        public Physician isPhysician(string AspId);

        public User isPatient(string AspId);

        public string GetAspId(string email);

        public List<Rolemenu> rolemenus(int roleId);

        public AspNetUser asp(string aspId);

        public void ResetPassword(ResetPassword obj);
    }
}
