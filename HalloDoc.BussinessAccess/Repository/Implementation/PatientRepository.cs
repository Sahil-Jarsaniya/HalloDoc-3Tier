using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.ViewModel;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _db;

        public PatientRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public DashboardViewModel PatientDashboard(String AspId)
        {
            var patientAspId = _db.Users.Where(x => x.Aspnetuserid == AspId).FirstOrDefault();
            var userId = patientAspId.Userid;

            var reqData = (from r in _db.Requests
                           where r.Userid == userId
                           join s in _db.RequestStatuses on r.Status equals s.StatusId
                           select new PatientDashboardViewModel
                           {
                               RequestId = r.Requestid,
                               fileCount = 1,
                               Status = s.Status,
                               Createddate = r.Createddate
                           }).ToList();



            var userdata = new ProfileEditViewModel
            {
                UserId = patientAspId.Userid,
                Firstname = patientAspId.Firstname,
                Lastname = patientAspId.Lastname,
                Email = patientAspId.Email,
                Phonenumber = patientAspId.Mobile,
                Strmonth = patientAspId.Strmonth,
                Street = patientAspId.Street,
                City = patientAspId.City,
                State = patientAspId.State,
                Zipcode = patientAspId.Zipcode
            };

            var data = new DashboardViewModel
            {
                PatientDashboardViewModel = reqData,
                ProfileEditViewModel = userdata
            };

            return data;
        }

    }
}
