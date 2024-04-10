using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.DataAccess.Models;
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
        public string GetHash(string text);

        public AspNetUser GetLoginData(AspNetUser obj, String hashPass);

        public User PatientLogin(AspNetUser obj, String hashPass);

        public void SendEmail(String email, string subject, string body);

        public void uploadFile(IFormFile? fileName,string folder, string path);

        public Admin isAdmin(string AspId);
        public Physician isPhysician(string AspId);
    }
}
