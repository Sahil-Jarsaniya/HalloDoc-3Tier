using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _db;

        public LoginRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public Admin GetLoginData(AspNetUser obj, String hashPass)
        {
                var myUser = _db.AspNetUsers.Where(x => x.UserName == obj.UserName && x.PasswordHash == hashPass).FirstOrDefault();
                if (myUser == null)
                {
                    return null;
                }
                else
                {
                    var userId = _db.Admins.Where(x => x.Aspnetuserid == myUser.Id).FirstOrDefault();
                    return userId;
                }
        }

        public User PatientLogin(AspNetUser obj, String hashPass)
        {
            var myUser = _db.AspNetUsers.Include(x => x.Roles).Where(x => x.UserName == obj.UserName && x.PasswordHash == hashPass).FirstOrDefault();
            if (myUser == null)
            {
                return null;
            }
            else
            {
                var userId = _db.Users.Where(x => x.Aspnetuserid == myUser.Id).FirstOrDefault();
                return userId;
            }
        }

        public void SendEmail(string email, string subject, string body)
        {
            var emailToSend = new MimeMessage();

            emailToSend.From.Add(MailboxAddress.Parse("tatva.dotnet.sahiljarsaniya@outlook.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };    

            //send mail
            using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                emailClient.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("tatva.dotnet.sahiljarsaniya@outlook.com", "$@hilpj1");
                emailClient.Send(emailToSend);

                emailClient.Disconnect(true);
            }
        }

        public void uploadFile(IFormFile? file,string folder, string path)
        {
            //uploading files
            if (file != null && file.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(file.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles",folder, path);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }
    }
}
