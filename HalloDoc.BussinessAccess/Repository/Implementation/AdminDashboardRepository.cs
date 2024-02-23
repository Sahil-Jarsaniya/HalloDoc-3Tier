using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class AdminDashboardRepository: IAdminDashboardRepository
    {
        private readonly ApplicationDbContext _db;

        public AdminDashboardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public AdminDashboardViewModel adminDashboard()
        {
            

            var newCount = (from t1 in _db.Requests
                            where t1.Status == 1
                            select t1
                            ).Count();
            var pendingCount = (from t1 in _db.Requests
                                where t1.Status == 2
                                select t1
                            ).Count();
            var activeCount = (from t1 in _db.Requests
                               where t1.Status == 8
                               select t1
                            ).Count();
            var concludeCount = (from t1 in _db.Requests
                                 where t1.Status == 4
                                 select t1
                            ).Count();
            var closeCount = (from t1 in _db.Requests
                              where t1.Status == 5
                              select t1
                            ).Count();
            var unpaidCount = (from t1 in _db.Requests
                               where t1.Status == 13
                               select t1
                            ).Count();
            var count = new countRequestViewModel
            {
                newCount = newCount,
                pendingCount = pendingCount,
                activeCount = activeCount,
                concludeCount = concludeCount,
                closeCount = closeCount,
                unpaidCount = unpaidCount
            };

            var newReqData =( from req in _db.Requests
                          join rc in _db.Requestclients on req.Requestid equals rc.Requestid 
                          where req.Status == 1 
                          select new newReqViewModel
                          {
                              reqClientId = rc.Requestclientid,
                              Firstname = rc.Firstname,
                              Lastname = rc.Lastname,
                              reqFirstname = req.Firstname,
                              reqLastname = req.Lastname,
                              Strmonth = rc.Strmonth,
                              Createddate = req.Createddate,
                              Phonenumber = rc.Phonenumber,
                              ConciergePhonenumber = req.Phonenumber,
                              FamilyPhonenumber = req.Phonenumber,
                              BusinessPhonenumber = req.Phonenumber,
                              Street = rc.Street,
                              City = rc.City,
                              State = rc.State,
                              Zipcode = rc.Zipcode,
                              Notes = rc.Notes,
                              reqTypeId = req.Requesttypeid
                          });
            var pendingReqData = from req in _db.Requests
                             join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                             join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                             where req.Status == 2
                             select new pendingReqViewModel
                             {
                                 reqClientId = rc.Requestclientid,
                                 Firstname = rc.Firstname,
                                 Lastname = rc.Lastname,
                                 reqFirstname = req.Firstname,
                                 reqLastname = req.Lastname,
                                 Strmonth = rc.Strmonth,
                                 Createddate = req.Createddate,
                                 Phonenumber = rc.Phonenumber,
                                 ConciergePhonenumber = req.Phonenumber,
                                 FamilyPhonenumber = req.Phonenumber,
                                 BusinessPhonenumber = req.Phonenumber,
                                 Street = rc.Street,
                                 City = rc.City,
                                 State = rc.State,
                                 Zipcode = rc.Zipcode,
                                 Notes = rc.Notes,
                                 reqTypeId = req.Requesttypeid,
                                 physicianName = phy.Firstname + " " + phy.Lastname
                             };
            var closeReqData = from req in _db.Requests
                             join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                             join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                             where req.Status == 5
                             select new closeReqViewModel
                             {
                                 reqClientId = rc.Requestclientid,
                                 Firstname = rc.Firstname,
                                 Lastname = rc.Lastname,
                                 reqFirstname = req.Firstname,
                                 reqLastname = req.Lastname,
                                 Strmonth = rc.Strmonth,
                                 Createddate = req.Createddate,
                                 Phonenumber = rc.Phonenumber,
                                 ConciergePhonenumber = req.Phonenumber,
                                 FamilyPhonenumber = req.Phonenumber,
                                 BusinessPhonenumber = req.Phonenumber,
                                 Street = rc.Street,
                                 City = rc.City,
                                 State = rc.State,
                                 Zipcode = rc.Zipcode,
                                 Notes = rc.Notes,
                                 reqTypeId = req.Requesttypeid,
                                 physicianName = phy.Firstname + " " + phy.Lastname
                             };
            var concludeReqData = from req in _db.Requests
                             join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                             join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                             where req.Status == 4
                             select new concludeReqViewModel
                             {
                                 reqClientId = rc.Requestclientid,
                                 Firstname = rc.Firstname,
                                 Lastname = rc.Lastname,
                                 reqFirstname = req.Firstname,
                                 reqLastname = req.Lastname,
                                 Strmonth = rc.Strmonth,
                                 Createddate = req.Createddate,
                                 Phonenumber = rc.Phonenumber,
                                 ConciergePhonenumber = req.Phonenumber,
                                 FamilyPhonenumber = req.Phonenumber,
                                 BusinessPhonenumber = req.Phonenumber,
                                 Street = rc.Street,
                                 City = rc.City,
                                 State = rc.State,
                                 Zipcode = rc.Zipcode,
                                 Notes = rc.Notes,
                                 reqTypeId = req.Requesttypeid,
                                 physicianName = phy.Firstname + " " + phy.Lastname
                             };
            var unpaidReqData = from req in _db.Requests
                             join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                             join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                             where req.Status == 13
                             select new unpaidReqViewModel
                             {
                                 reqClientId = rc.Requestclientid,
                                 Firstname = rc.Firstname,
                                 Lastname = rc.Lastname,
                                 reqFirstname = req.Firstname,
                                 reqLastname = req.Lastname,
                                 Strmonth = rc.Strmonth,
                                 Createddate = req.Createddate,
                                 Phonenumber = rc.Phonenumber,
                                 ConciergePhonenumber = req.Phonenumber,
                                 FamilyPhonenumber = req.Phonenumber,
                                 BusinessPhonenumber = req.Phonenumber,
                                 Street = rc.Street,
                                 City = rc.City,
                                 State = rc.State,
                                 Zipcode = rc.Zipcode,
                                 Notes = rc.Notes,
                                 reqTypeId = req.Requesttypeid,
                                 physicianName = phy.Firstname + " " + phy.Lastname
                             };
            var activeReqData = from req in _db.Requests
                                join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                                join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                                where req.Status == 8
                                select new activeReqViewModel
                                {
                                    reqClientId = rc.Requestclientid,
                                    Firstname = rc.Firstname,
                                    Lastname = rc.Lastname,
                                    reqFirstname = req.Firstname,
                                    reqLastname = req.Lastname,
                                    Strmonth = rc.Strmonth,
                                    Createddate = req.Createddate,
                                    Phonenumber = rc.Phonenumber,
                                    ConciergePhonenumber = req.Phonenumber,
                                    FamilyPhonenumber = req.Phonenumber,
                                    BusinessPhonenumber = req.Phonenumber,
                                    Street = rc.Street,
                                    City = rc.City,
                                    State = rc.State,
                                    Zipcode = rc.Zipcode,
                                    Notes = rc.Notes,
                                    reqTypeId = req.Requesttypeid,
                                    physicianName = phy.Firstname + " " + phy.Lastname
                                };




            var data = new AdminDashboardViewModel
            {
                countRequestViewModel = count,
                newReqViewModel = newReqData,
                concludeReqViewModel = concludeReqData,
                closeReqViewModels = closeReqData,
                activeReqViewModels = activeReqData,
                pendingReqViewModel = pendingReqData,
                unpaidReqViewModels = unpaidReqData
                
            };
            return data;
        }

        public viewCaseViewModel viewCase(int reqClientId)
        {
            var data = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            var cNumber = _db.Requests.FirstOrDefault(x => x.Requestid == data.Requestid);
            var confirm = cNumber.Confirmationnumber;

            var viewdata = new viewCaseViewModel
            {
                Requestclientid = reqClientId,
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Strmonth = data.Strmonth,
                confirmationNumber = confirm,
                Notes = data.Notes,
                Address = data.Address,
                Email = data.Email,
                Phonenumber = data.Phonenumber,
                City = data.City,
                State = data.State,
                Street = data.Street,
                Zipcode = data.Zipcode,
                Regionid = data.Regionid,
            };

            return viewdata;
        }

        public bool viewCase(viewCaseViewModel obj)
        {
            try
            {
                var rcId = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == obj.Requestclientid);
                var rId = rcId.Requestid;
                var rRow = _db.Requests.FirstOrDefault(x => x.Requestid == rId);
                var uid = rRow.Userid;
                var uRow = _db.Users.FirstOrDefault(x => x.Userid == uid);
                var aspId = uRow.Aspnetuserid;
                var aspRow = _db.AspNetUsers.FirstOrDefault(x => x.Id == aspId);

                var email = aspRow.Email;


                uRow.Firstname = obj.Firstname;
                uRow.Lastname = obj.Lastname;
                uRow.Email = obj.Email;
                uRow.Mobile = obj.Phonenumber;
                uRow.Strmonth = obj.Strmonth;

                _db.Users.Update(uRow);
                _db.SaveChanges();


                aspRow.Email = obj.Email;
                aspRow.UserName = obj.Email;
                aspRow.PhoneNumber = obj.Phonenumber;
                aspRow.ModifiedDate = DateTime.UtcNow;

                _db.AspNetUsers.Update(aspRow);
                _db.SaveChanges();

                _db.Requestclients.Where(x => x.Email == email).ToList().ForEach(item =>
                {
                    item.Firstname = obj.Firstname;
                    item.Lastname = obj.Lastname;
                    item.Email = obj.Email;
                    item.Phonenumber = obj.Phonenumber;
                    item.Address = obj.Address;
                    item.Strmonth = obj.Strmonth;

                    _db.Requestclients.Update(item);
                    _db.SaveChanges();
                });

                _db.Requests.Where(x => x.Email == email).ToList().ForEach(item =>
                {
                    item.Firstname = obj.Firstname;
                    item.Lastname = obj.Lastname;
                    item.Email = obj.Email;
                    item.Phonenumber = obj.Phonenumber;
                    item.Modifieddate = DateTime.UtcNow;
                    _db.Requests.Update(item);
                    _db.SaveChanges();
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
