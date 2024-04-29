using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using Microsoft.AspNetCore.Html;
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

        public bool isEmailAvailable(string email)
        {
            return _db.AspNetUsers.Any(u => u.Email == email);
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

        public string GetAspId(string email)
        {
            var myUser = _db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
            return myUser.Id;
        }
        public AspNetUser asp(string aspId)
        {
            return _db.AspNetUsers.FirstOrDefault(x => x.Id == aspId);
        }

        public AspNetUser GetLoginData(login obj, String hashPass)
        {
            var myUser = _db.AspNetUsers.Where(x => x.UserName == obj.Username && x.PasswordHash == hashPass).FirstOrDefault();
            if (myUser == null)
            {
                return null;
            }
            else
            {
                return myUser;
            }
        }
        public Admin isAdmin(string AspId)
        {
            var isAdmin = _db.Admins.FirstOrDefault(x => x.Aspnetuserid == AspId);
            if (isAdmin != null)
            {
                var isRoleDeleted = _db.Roles.FirstOrDefault(x => x.Roleid == isAdmin.Roleid).Isdeleted;
                if (!isRoleDeleted)
                {
                    return isAdmin;

                }
            }
            return null;
        }
        public Physician isPhysician(string AspId)
        {
            var isPhysician = _db.Physicians.FirstOrDefault(x => x.Aspnetuserid == AspId);
            if (isPhysician != null)
            {
                var isRoleDeleted = _db.Roles.FirstOrDefault(x => x.Roleid == isPhysician.Roleid).Isdeleted;

                if (!isRoleDeleted)
                {
                    return isPhysician;

                }
            }
            return null;
        }

        public User isPatient(string AspId)
        {
            var isPatient = _db.Users.FirstOrDefault(x => x.Aspnetuserid == AspId);

            return isPatient;
        }

        public void SendEmail(string email, string subject, string body, string[]? attachments)
        {
            var emailLog = new Emaillog()
            {
                Emailid = email,
                Createdate = DateTime.Now,
                Subjectname = subject,
                Emailtemplate = "a"
            };
            _db.Emaillogs.Add(emailLog);
            _db.SaveChanges();
            try
            {
                var emailToSend = new MimeMessage();

                var builder = new BodyBuilder();

                emailToSend.From.Add(MailboxAddress.Parse("tatva.dotnet.sahiljarsaniya@outlook.com"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                System.Net.Mail.Attachment attachment;
                if (attachments != null)
                {
                    for (int i = 0; i < attachments.Length; i++)
                    {
                        var folder = attachments[i].Split("/");
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", "RequestData", folder[0], folder[1]);
                        builder.Attachments.Add(filePath);
                    }
                    emailToSend.Body = builder.ToMessageBody();
                }
                //send mail
                using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    emailClient.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    emailClient.Authenticate("tatva.dotnet.sahiljarsaniya@outlook.com", "$@hilpj1");
                    emailClient.Send(emailToSend);

                    emailClient.Disconnect(true);
                }
                emailLog.Sentdate = DateTime.Now;
                emailLog.Senttries = emailLog.Senttries == null ? 1 : emailLog.Senttries + 1;
                emailLog.Isemailsent = true;
                _db.Emaillogs.Update(emailLog);
                _db.SaveChanges();
            }
            catch
            {
                emailLog.Senttries = emailLog.Senttries == null ? 1 : emailLog.Senttries + 1;
                emailLog.Isemailsent = false;
                _db.Emaillogs.Update(emailLog);
                _db.SaveChanges();
            }
        }

        public void uploadFile(IFormFile? file, string folder, string path)
        {
            //uploading files
            if (file != null && file.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(file.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", folder);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    Console.WriteLine("Folder created successfully.");
                }

                var fileaaaa = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", folder, fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(fileaaaa, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }

        public void ResetPassword(ResetPassword obj)
        {
            var aspUser = _db.AspNetUsers.Where(x => x.Email == obj.email && x.Id == obj.Id).FirstOrDefault();
            aspUser.PasswordHash = GetHash(obj.Password);
            aspUser.ModifiedDate = DateTime.UtcNow;
            _db.AspNetUsers.Update(aspUser);
            _db.SaveChanges();
        }

        public List<Rolemenu> rolemenus(int roleId)
        {
            return _db.Rolemenus.Where(x => x.Roleid == roleId).ToList();
        }
    }
}
