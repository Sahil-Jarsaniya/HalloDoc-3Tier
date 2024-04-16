using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop.Implementation;
using Org.BouncyCastle.Ocsp;
using System.Linq;
using System.Xml.Linq;
using Twilio.Rest.Chat.V1.Service;
using Twilio.TwiML.Voice;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly ApplicationDbContext _db;

        public AdminDashboardRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public int GetAdminId(string AspId)
        {
            return _db.Admins.Where(x => x.Aspnetuserid == AspId).FirstOrDefault().Adminid;
        }

        public List<Menu> Menus()
        {
            return _db.Menus.ToList();
        }
        public List<AccountType> AccountType()
        {
            return _db.AccountTypes.ToList();
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
                               where t1.Status == 8 || t1.Status == 15
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

            var CaseTag = from t1 in _db.Casetags select t1;
            var Region = from t1 in _db.Regions select t1;

            var data = new AdminDashboardViewModel
            {
                countRequestViewModel = count,
                Casetag = CaseTag,
                Region = Region
            };
            return data;
        }

        public IQueryable<newReqViewModel> newReq()
        {
            var newReqData = (from req in _db.Requests
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
                                  Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).OrderByDescending(x => x.Createddate).Select(x => x.Notes).FirstOrDefault() ?? "-",
                                  reqTypeId = req.Requesttypeid,
                                  Regionid = rc.Regionid,
                                  Email = rc.Email,
                              });
            return newReqData;
        }
        public IQueryable<pendingReqViewModel> pendingReq()
        {
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
                                     Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid ).OrderByDescending(x => x.Createddate).Select(x=> x.Notes).FirstOrDefault() ?? "-",
                                     reqTypeId = req.Requesttypeid,
                                     physicianName = phy.Firstname + " " + phy.Lastname,
                                     Physicianid = phy.Physicianid,
                                     Regionid = rc.Regionid,
                                     Email = rc.Email,
                                 };
            return pendingReqData;
        }
        public IQueryable<activeReqViewModel> activeReq()
        {
            var activeReqData = from req in _db.Requests
                                join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                                join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                                where req.Status == 8 || req.Status == 15
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
                                    Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).OrderByDescending(x => x.Createddate).Select(x => x.Notes).FirstOrDefault() ?? "-",
                                    reqTypeId = req.Requesttypeid,
                                    physicianName = phy.Firstname + " " + phy.Lastname,
                                    Regionid = rc.Regionid,
                                    Email = rc.Email,
                                };
            return activeReqData;
        }
        public IQueryable<concludeReqViewModel> concludeReq()
        {
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
                                      physicianName = phy.Firstname + " " + phy.Lastname,
                                      Regionid = rc.Regionid,
                                      Email = rc.Email,
                                  };
            return concludeReqData;
        }
        public IQueryable<closeReqViewModel> closeReq()
        {
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
                                   Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).OrderByDescending(x => x.Createddate).Select(x => x.Notes).FirstOrDefault() ?? "-",
                                   reqTypeId = req.Requesttypeid,
                                   physicianName = phy.Firstname + " " + phy.Lastname,
                                   Regionid = rc.Regionid,
                                   Email = rc.Email,
                               };

            return closeReqData;
        }
        public IQueryable<unpaidReqViewModel> unpaidReq()
        {
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
                                    physicianName = phy.Firstname + " " + phy.Lastname,
                                    Regionid = rc.Regionid,
                                    Email = rc.Email,
                                };
            return unpaidReqData;
        }

        public IQueryable<newReqViewModel> newReq(searchViewModel obj)
        {
            var newReqData = (from req in _db.Requests
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
                                  Notes = rc.Notes ?? "-",
                                  reqTypeId = req.Requesttypeid,
                                  Regionid = rc.Regionid,
                                  Email = rc.Email,
                              });

            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                newReqData = newReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
            }
            if (obj.RegionId != 0)
            {
                newReqData = newReqData.Where(s => s.Regionid == obj.RegionId);
            }
            if (obj.reqType != 0)
            {
                newReqData = newReqData.Where(s => s.reqTypeId == obj.reqType);
            }

            return newReqData;
        }
        public IQueryable<pendingReqViewModel> pendingReq(searchViewModel obj)
        {
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
                                     Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).OrderByDescending(x => x.Createddate).Select(x => x.Notes).FirstOrDefault() ?? "-",
                                     reqTypeId = req.Requesttypeid,
                                     physicianName = phy.Firstname + " " + phy.Lastname,
                                     Physicianid = phy.Physicianid,
                                     Regionid = rc.Regionid,
                                     Email = rc.Email,
                                 };
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                pendingReqData = pendingReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
            }
            if (obj.RegionId != 0)
            {
                pendingReqData = pendingReqData.Where(s => s.Regionid == obj.RegionId);
            }
            if (obj.reqType != 0)
            {
                pendingReqData = pendingReqData.Where(s => s.reqTypeId == obj.reqType);
            }

            return pendingReqData;
        }
        public IQueryable<activeReqViewModel> activeReq(searchViewModel obj)
        {
            var activeReqData = from req in _db.Requests
                                join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                                join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                                where req.Status == 8 || req.Status == 15
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
                                    Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).OrderByDescending(x => x.Createddate).Select(x => x.Notes).FirstOrDefault() ?? "-",
                                    reqTypeId = req.Requesttypeid,
                                    physicianName = phy.Firstname + " " + phy.Lastname,
                                    Regionid = rc.Regionid,
                                    Email = rc.Email,
                                };

            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                activeReqData = activeReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
            }
            if (obj.RegionId != 0)
            {
                activeReqData = activeReqData.Where(s => s.Regionid == obj.RegionId);
            }
            if (obj.reqType != 0)
            {
                activeReqData = activeReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return activeReqData;
        }
        public IQueryable<concludeReqViewModel> concludeReq(searchViewModel obj)
        {
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
                                      physicianName = phy.Firstname + " " + phy.Lastname,
                                      Regionid = rc.Regionid,
                                      Email = rc.Email,
                                  };
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                concludeReqData = concludeReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
            }
            if (obj.RegionId != 0)
            {
                concludeReqData = concludeReqData.Where(s => s.Regionid == obj.RegionId);
            }
            if (obj.reqType != 0)
            {
                concludeReqData = concludeReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return concludeReqData;
        }
        public IQueryable<closeReqViewModel> closeReq(searchViewModel obj)
        {
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
                                   Notes = _db.Requeststatuslogs.Where(x => x.Requestid == req.Requestid ).OrderByDescending(x => x.Createddate).Select(x => x.Notes).FirstOrDefault() ?? "-",
                                   reqTypeId = req.Requesttypeid,
                                   physicianName = phy.Firstname + " " + phy.Lastname,
                                   Regionid = rc.Regionid,
                                   Email = rc.Email,
                               };

            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                closeReqData = closeReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
            }
            if (obj.RegionId != 0)
            {
                closeReqData = closeReqData.Where(s => s.Regionid == obj.RegionId);
            }
            if (obj.reqType != 0)
            {
                closeReqData = closeReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return closeReqData;
        }
        public IQueryable<unpaidReqViewModel> unpaidReq(searchViewModel obj)
        {
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
                                    physicianName = phy.Firstname + " " + phy.Lastname,
                                    Regionid = rc.Regionid,
                                    Email = rc.Email,
                                };
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                unpaidReqData = unpaidReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
            }
            if (obj.RegionId != 0)
            {
                unpaidReqData = unpaidReqData.Where(s => s.Regionid == obj.RegionId);
            }
            if (obj.reqType != 0)
            {
                unpaidReqData = unpaidReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return unpaidReqData;
        }


        public viewCaseViewModel viewCase(int reqClientId)
        {
            var data = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            var cNumber = _db.Requests.FirstOrDefault(x => x.Requestid == data.Requestid);
            var confirm = cNumber.Confirmationnumber;
            var Casetag = from t1 in _db.Casetags select t1;
            var region = from t1 in _db.Regions select t1;
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
                status = cNumber.Status,
                Casetag = Casetag,
                Region = region,
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


                uRow.Email = obj.Email;
                uRow.Mobile = obj.Phonenumber;

                _db.Users.Update(uRow);
                _db.SaveChanges();


                aspRow.Email = obj.Email;
                aspRow.PhoneNumber = obj.Phonenumber;
                aspRow.ModifiedDate = DateTime.UtcNow;

                _db.AspNetUsers.Update(aspRow);
                _db.SaveChanges();

                _db.Requestclients.Where(x => x.Email == email).ToList().ForEach(item =>
                {
                    item.Email = obj.Email;
                    item.Phonenumber = obj.Phonenumber;
                    _db.Requestclients.Update(item);
                    _db.SaveChanges();
                });

                _db.Requests.Where(x => x.Email == email).ToList().ForEach(item =>
                {
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

        public viewNoteViewModel ViewNoteGet(int reqClientId)
        {
            var ReqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqNotes = from t1 in _db.Requestnotes
                           where t1.Requestid == ReqId.Requestid
                           select t1;

            var transferNote = from t1 in _db.Requeststatuslogs
                               where t1.Requestid == ReqId.Requestid
                               select t1;
            var status = _db.Requests.Where(x => x.Requestid == ReqId.Requestid).FirstOrDefault().Status;
            var data = new viewNoteViewModel
            {
                status = status,
                reqClientId = reqClientId,
                Requestnote = reqNotes,
                Requeststatuslog = transferNote
            };

            return data;
        }

        public void ViewNotePost(int reqClientId, string adminNote, int adminId)
        {
            var reqIdCol = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqId = reqIdCol.Requestid;
            var Status = _db.Requests.Where(x => x.Requestid == reqId).FirstOrDefault().Status;

            var reqNote = _db.Requestnotes.Where(x => x.Requestid == reqId).FirstOrDefault();
            var adminAspId = _db.Admins.Where(x => x.Adminid == adminId).FirstOrDefault().Aspnetuserid;

            if (reqNote == null)
            {
                var reqNoteDb = new Requestnote
                {
                    Requestid = (int)reqId,
                    Adminnotes = adminNote,
                    Createddate = DateTime.Now,
                    Createdby = adminAspId
                };
                _db.Requestnotes.Add(reqNoteDb);
                _db.SaveChanges();
            }
            else
            {
                var reqNoteDb = _db.Requestnotes.Where(x => x.Requestid == reqId).FirstOrDefault();

                reqNoteDb.Requestid = (int)reqId;
                reqNoteDb.Adminnotes = adminNote;
                reqNoteDb.Modifieddate = DateTime.Now;
                reqNoteDb.Modifiedby = adminAspId;
                _db.Requestnotes.Update(reqNoteDb);
                _db.SaveChanges();
            }

            var reqStatusLog = new Requeststatuslog
            {
                Requestid = (int)reqId,
                Status = Status,
                Adminid = adminId,
                Notes = adminNote,
                Createddate = DateTime.Now,
            };
            _db.Requeststatuslogs.Add(reqStatusLog);
            _db.SaveChanges();
        }

        public void CancelCase(int CaseTag, string addNote, int reqClientId, int adminId)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqCol = _db.Requests.Where(x => x.Requestid == reqId.Requestid).FirstOrDefault();

            reqCol.Status = 5;
            reqCol.Casetag = _db.Casetags.Where(x => x.Casetagid == CaseTag).FirstOrDefault().Name;
            _db.Requests.Update(reqCol);
            _db.SaveChanges();

            var reqStatuslog = new Requeststatuslog
            {
                Requestid = (int)reqId.Requestid,
                Notes = addNote,
                Createddate = DateTime.Now,
                Adminid = adminId,
                Status = 5
            };
            _db.Requeststatuslogs.Add(reqStatuslog);
            _db.SaveChanges();
        }

        public void BlockCase(int reqClientId, string addNote, int adminId)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqCol = _db.Requests.Where(x => x.Requestid == reqId.Requestid).FirstOrDefault();
            var userRow = _db.Users.FirstOrDefault(x => x.Userid == reqCol.Userid);

            reqCol.Status = 14;
            _db.Requests.Update(reqCol);
            _db.SaveChanges();

            var reqStatuslog = new Requeststatuslog
            {
                Requestid = (int)reqId.Requestid,
                Notes = addNote,
                Createddate = DateTime.Now,
                Adminid = adminId,
                Status = 14
            };
            _db.Requeststatuslogs.Add(reqStatuslog);
            _db.SaveChanges();

            var Block = _db.Blockrequests.FirstOrDefault(x => x.Requestid == reqCol.Requestid);

            if (Block == null)
            {

                var reqBlock = new Blockrequest
                {
                    Phonenumber = reqId.Phonenumber,
                    Email = reqId.Email,
                    Reason = addNote,
                    Requestid = reqCol.Requestid,
                    Createddate = DateTime.Now,
                    Isactive = false
                };
                _db.Blockrequests.Add(reqBlock);
                _db.SaveChanges();
            }
            else
            {
                Block.Isactive = false;
                _db.Blockrequests.Update(Block);
                _db.SaveChanges();
            }
        }

        public object FilterPhysician(int Region, int PhyId)
        {
            var physicians = (from t1 in _db.Physicianregions
                              join t2 in _db.Physicians on t1.Physicianid equals t2.Physicianid
                              where t1.Regionid == Region && t2.Physicianid != PhyId
                              select new
                              {
                                  physicians = t2.Firstname + " " + t2.Lastname,
                                  PhysicianId = t1.Physicianid
                              }).ToList();
            return physicians;
        }

        public void AssignCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect, int adminId, string AspId)
        {
            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqClientRow.Requestid).FirstOrDefault();
            reqRow.Status = 1;
            reqRow.Physicianid = PhysicianSelect;
            reqRow.Modifieddate = DateTime.Now;
            _db.Requests.Update(reqRow);
            _db.SaveChanges();

            var reqNote = _db.Requestnotes.Where(x => x.Requestid == reqRow.Requestid).FirstOrDefault();

            if (reqNote == null)
            {
                reqNote = new Requestnote
                {
                    Requestid = reqRow.Requestid,
                    Adminnotes = addNote,
                    Createddate = DateTime.Now,
                    Createdby = AspId
                };
                _db.Requestnotes.Add(reqNote);
                _db.SaveChanges();
            }
            else
            {
                reqNote.Adminnotes = addNote;
                reqNote.Modifieddate = DateTime.Now;
                reqNote.Modifiedby = AspId;
                _db.Requestnotes.Update(reqNote);
                _db.SaveChanges();
            }


            var reqStatusLog = new Requeststatuslog
            {
                Createddate = DateTime.Now,
                Requestid = reqRow.Requestid,
                Adminid = adminId,
                Notes = addNote,
                Status = 2
            };
            _db.Requeststatuslogs.Add(reqStatusLog);
            _db.SaveChanges();

        }

        public DocumentViewModel ViewUpload(int reqClientId)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).First();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqId.Requestid).First();
            var requestData = from t1 in _db.Requests
                              join t3 in _db.RequestStatuses on t1.Status equals t3.StatusId
                              join t2 in _db.Requestwisefiles
                              on t1.Requestid equals t2.Requestid into files
                              from t2 in files.DefaultIfEmpty()
                              where t1.Requestid == reqId.Requestid
                              select new PatientDocumentViewModel
                              {
                                  RequestId = t1.Requestid,
                                  ReqClientId = reqClientId,
                                  Name = t1.Firstname + " " + t1.Lastname,
                                  createdate = t1.Createddate,
                                  Filename = t2 != null ? t2.Filename : null,
                                  IsDeleted = t2.Isdeleted
                              };
            var uploadData = new UploadFileViewModel
            {
                reqId = reqClientId,
                formFile = null
            };
            var data = new DocumentViewModel
            {
                status = _db.Requests.Where(x => x.Requestid == reqId.Requestid).First().Status,
                PatientName = reqId.Firstname + " " + reqId.Lastname,
                ConfirmationNo = reqRow.Confirmationnumber,
                PatientDocumentViewModel = requestData,
                UploadFileViewModel = uploadData
            };

            return data;
        }
        public void ViewUploadFile(string file, int reqId, int adminId)
        {
            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == reqId).FirstOrDefault();
            Requestwisefile requestwisefile = new Requestwisefile
            {
                Filename = file,
                Requestid = (int)reqClientRow.Requestid,
                Createddate = DateTime.Now,
                Adminid = adminId
            };

            _db.Requestwisefiles.Add(requestwisefile);
            _db.SaveChanges();
        }

        public void DeleteFile(int reqClientId, string FileName)
        {
            var requestId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault().Requestid;
            var rwf = _db.Requestwisefiles.Where(x => x.Requestid == requestId && x.Filename == FileName).FirstOrDefault();
            rwf.Isdeleted = true;
            _db.Requestwisefiles.Update(rwf);
            _db.SaveChanges();
        }

        public SendOrderViewModel SendOrders(int reqClientId)
        {
            var reqCRow = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var status = _db.Requests.Where(x => x.Requestid == reqCRow.Requestid).FirstOrDefault().Status;

            var sendOrder = new SendOrderViewModel
            {
                Healthprofessionaltype = _db.Healthprofessionaltypes,
                reqClientId = reqClientId,
                status = status
            };

            return sendOrder;
        }

        public void SendOrders(SendOrderViewModel obj, string AspId)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == obj.reqClientId).FirstOrDefault().Requestid;

            var order = new Orderdetail
            {
                Requestid = reqId,
                Vendorid = obj.ProfessionalId,
                Faxnumber = obj.FaxNumber,
                Email = obj.email,
                Businesscontact = obj.ProfessionalPhone,
                Prescription = obj.OrderDetail,
                Noofrefill = obj.noOfRefill,
                Createddate = DateTime.Now,
                Createdby = AspId
            };
            _db.Orderdetails.Add(order);
            _db.SaveChanges();
        }

        public object FilterProfession(int ProfessionId)
        {
            var data = (from t1 in _db.Healthprofessionals
                        join t2 in _db.Healthprofessionaltypes on t1.Profession equals t2.Healthprofessionalid
                        where t2.Healthprofessionalid == ProfessionId
                        select new
                        {
                            vendorname = t1.Vendorname,
                            vendorid = t1.Vendorid
                        }).ToList();

            return data;
        }

        public object ShowVendorDetail(int selectVendor)
        {
            var data = (from t1 in _db.Healthprofessionals
                        where t1.Vendorid == selectVendor
                        select new
                        {
                            vendorPhone = t1.Phonenumber,
                            email = t1.Email,
                            faxNumber = t1.Faxnumber
                        }).ToList();

            return data;
        }

        public void ClearCase(int reqClientId)
        {
            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqClientRow.Requestid).FirstOrDefault();
            reqRow.Status = 9;
            _db.Update(reqRow);
            _db.SaveChanges();
        }

        public CloseCaseViewModel CloseCase(int reqClientId)
        {
            var reqId = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).First();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqId.Requestid).First();

            var requestData = from t1 in _db.Requests
                              join t3 in _db.RequestStatuses on t1.Status equals t3.StatusId
                              join t2 in _db.Requestwisefiles
                              on t1.Requestid equals t2.Requestid into files
                              from t2 in files.DefaultIfEmpty()
                              where t1.Requestid == reqId.Requestid
                              select new PatientDocumentViewModel
                              {
                                  RequestId = t1.Requestid,
                                  ReqClientId = reqClientId,
                                  Name = t1.Firstname + " " + t1.Lastname,
                                  createdate = t1.Createddate,
                                  Filename = t2 != null ? t2.Filename : null,
                                  IsDeleted = t2.Isdeleted
                              };

            var data = new CloseCaseViewModel
            {
                ReqClientId = reqId.Requestclientid,
                confirmationNo = reqRow.Confirmationnumber,
                status = reqRow.Status,
                Firstname = reqId.Firstname,
                Lastname = reqId.Lastname,
                Phonenumber = reqId.Phonenumber,
                Email = reqId.Email,
                Strmonth = reqId.Strmonth,
                PatientDocumentViewModel = requestData
            };

            return data;
        }

        public void CloseCase(CloseCaseViewModel obj)
        {
            var ReqClientRow = _db.Requestclients.Where(x => x.Requestclientid == obj.ReqClientId).FirstOrDefault();
            ReqClientRow.Email = obj.Email;
            ReqClientRow.Phonenumber = obj.Phonenumber;
            _db.Requestclients.Update(ReqClientRow);
            _db.SaveChanges();

            var ReqRow = _db.Requests.Where(x => x.Requestid == ReqClientRow.Requestid).FirstOrDefault();
            ReqRow.Phonenumber = obj.Phonenumber;
            ReqRow.Email = obj.Email;
            _db.Requests.Update(ReqRow);
            _db.SaveChanges();

            var UserRow = _db.Users.Where(x => x.Userid == ReqRow.Userid).FirstOrDefault();
            UserRow.Email = obj.Email;
            UserRow.Mobile = obj.Phonenumber;
            _db.Users.Update(UserRow);
            _db.SaveChanges();

            var AspRow = _db.AspNetUsers.Where(x => x.Id == UserRow.Aspnetuserid).FirstOrDefault();
            AspRow.Email = obj.Email;
            AspRow.PhoneNumber = obj.Phonenumber;
            _db.AspNetUsers.Update(AspRow);
            _db.SaveChanges();
        }

        public void CloseToUnpaidCase(int reqClientId)
        {
            var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqRow = _db.Requests.Where(x => x.Requestid == reqClientRow.Requestid).FirstOrDefault();
            reqRow.Status = 13;
            _db.Requests.Update(reqRow);
            _db.SaveChanges();
        }

        public Encounter Encounter(int reqClientId)
        {
            Requestclient? requestclient = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            Request? request = _db.Requests.Where(x => x.Requestid == requestclient.Requestid).FirstOrDefault();
            Encounter? encounter = _db.Encounters.Where(x => x.Requestid == request.Requestid).FirstOrDefault();

            if (encounter == null)
            {

                Encounter obj = new Encounter()
                {
                    Firstname = requestclient.Firstname,
                    LastName = requestclient.Lastname,
                    Email = requestclient.Email,
                    Phonenumber = requestclient.Phonenumber,
                    Strmonth = requestclient.Strmonth,
                    Location = requestclient.Location,
                    Requestid = request.Requestid
                };
                _db.Encounters.Add(obj);
                _db.SaveChanges();
                return obj;
            }
            else
            {
                return encounter;
            }
        }

        public void Encounter(Encounter obj)
        {
            Encounter encounter = _db.Encounters.Where(x => x.Requestid == obj.Requestid).FirstOrDefault();
            if (encounter != null)
            {
                encounter.Firstname = obj.Firstname;
                encounter.LastName = obj.LastName;
                encounter.Email = obj.Email;
                encounter.Phonenumber = obj.Phonenumber;
                encounter.Location = obj.Location;
                encounter.Strmonth = obj.Strmonth;
                encounter.Servicedate = obj.Servicedate;
                encounter.MedicalHistory = obj.MedicalHistory;
                encounter.PresentIllnessHistory = obj.PresentIllnessHistory;
                encounter.Medications = obj.Medications;
                encounter.Allergies = obj.Allergies;
                encounter.Temperature = obj.Temperature;
                encounter.HeartRate = obj.HeartRate;
                encounter.RespirationRate = obj.RespirationRate;
                encounter.BloodPressureDiastolic = obj.BloodPressureDiastolic;
                encounter.BloodPressureSystolic = obj.BloodPressureSystolic;
                encounter.OxygenLevel = obj.OxygenLevel;
                encounter.Pain = obj.Pain;
                encounter.Heent = obj.Heent;
                encounter.Chest = obj.Chest;
                encounter.Abdomen = obj.Abdomen;
                encounter.Extremities = obj.Extremities;
                encounter.Skin = obj.Skin;
                encounter.Neuro = obj.Neuro;
                encounter.Other = obj.Other;
                encounter.Diagnosis = obj.Diagnosis;
                encounter.TreatmentPlan = obj.TreatmentPlan;
                encounter.MedicationsDispensed = obj.MedicationsDispensed;
                encounter.Procedures = obj.Procedures;
                encounter.FollowUp = obj.FollowUp;

                _db.Encounters.Update(encounter);
                _db.SaveChanges();
            }
            }

        public int GetStatus(int reqClientId)
        {
            Requestclient? requestclient = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            Request? request = _db.Requests.Where(x => x.Requestid == requestclient.Requestid).FirstOrDefault();

            return request.Status;
        }

        public List<Physician> RequestSupportDTY()
        {
            return _db.Physicians.Where(x => x.Status == 4).ToList();
        }

        public Profile MyProfile(string AspId)
        {
            Admin adminRow = _db.Admins.Where(x => x.Aspnetuserid == AspId).FirstOrDefault();
            AspNetUser aspRow = _db.AspNetUsers.Where(x => x.Id == AspId).FirstOrDefault();
            var Region = from t1 in _db.Regions select t1;

            var regionData = from t1 in _db.Regions
                             select new CheckBoxData
                             {
                                 Id = t1.Regionid,
                                 value = t1.Abbreviation,
                                 Checked = _db.Adminregions.Any(x => x.Adminid == adminRow.Adminid && x.Regionid == t1.Regionid)
                             };

            Profile data = new Profile
            {
                Adminid = adminRow.Adminid,
                Firstname = adminRow.Firstname,
                Lastname = adminRow.Lastname,
                Email = adminRow.Email,
                Mobile = adminRow.Mobile,
                Address1 = adminRow.Address1,
                Address2 = adminRow.Address2,
                Altphone = adminRow.Altphone,
                ConfirmEmail = adminRow.Email,
                Regionid = adminRow.Regionid,
                Roleid = adminRow.Roleid,
                Status = adminRow.Status,
                UserName = aspRow.UserName,
                Region = regionData,
                Roles = _db.Roles.Where(x => x.Accounttype == 1),
                Statues = _db.PhysicianStatuses,
                City = adminRow.City,
                Zip = adminRow.Zip,
            };

            return data;
        }

        public void MyProfile(Profile obj, string AspId)
        {
            int adminId = obj.Adminid;
            Admin? adminRow = _db.Admins.Where(x => x.Adminid == adminId).FirstOrDefault();



            if (obj.Firstname == null)
            {
                adminRow.Altphone = obj.Altphone;
                adminRow.Address1 = obj.Address1;
                adminRow.Address2 = obj.Address2;
                adminRow.Status = obj.Status;
                adminRow.Zip = obj.Zip;
                adminRow.City = obj.City;
                adminRow.Modifieddate = DateTime.Now;
                adminRow.Modifiedby = AspId;
            }
            else
            {
                adminRow.Firstname = obj.Firstname;
                adminRow.Lastname = obj.Lastname;
                adminRow.Email = obj.Email;
                adminRow.Mobile = obj.Mobile;
                adminRow.Regionid = obj.Regionid;
                adminRow.Roleid = obj.Roleid;
                adminRow.Status = obj.Status;
                adminRow.Modifieddate = DateTime.Now;
                adminRow.Modifiedby = AspId;
            }
            _db.Admins.Update(adminRow);
            _db.SaveChanges();
        }

        public void ResetAdminPass(string pass, int adminId)
        {
            var adminRow = _db.Admins.Where(x => x.Adminid == adminId).FirstOrDefault();
            var aspnetRow = _db.AspNetUsers.Where(x => x.Id == adminRow.Aspnetuserid).FirstOrDefault();

            aspnetRow.PasswordHash = pass;
            _db.AspNetUsers.Update(aspnetRow);
            _db.SaveChanges();
        }

        public void AdminRegionUpdate(List<CheckBoxData> selectedRegion, int adminId)
        {
            foreach (var item in selectedRegion)
            {
                var data = _db.Adminregions.FirstOrDefault(x => x.Adminid == adminId && x.Regionid == item.Id);

                if (data == null)
                {
                    if (item.Checked)
                    {
                        var adminRegion = new Adminregion
                        {
                            Adminid = adminId,
                            Regionid = item.Id,
                        };
                        _db.Adminregions.Add(adminRegion);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    if (!item.Checked)
                    {
                        _db.Adminregions.Remove(data);
                        _db.SaveChanges();
                    }
                }
            }
        }

        public ProviderViewModel Provider()
        {
            var region = from t1 in _db.Regions select t1;
            var phy = from t1 in _db.Physicians
                      join t2 in _db.Physiciannotifications on t1.Physicianid equals t2.Physicianid 
                      join t3 in _db.Roles on t1.Roleid equals t3.Roleid 
                      join t4 in _db.PhysicianStatuses on t1.Status equals t4.StatusId 
                      select new ProviderTableViewModel
                      {
                          Firstname = t1.Firstname,
                          Lastname = t1.Lastname,
                          Email = t1.Email,
                          Mobile = t1.Mobile,
                          isNotiOff = t2.Isnotificationstopped,
                          Status = t4.StatusName,
                          Roleid = t3.Name,
                          Physicianid = t1.Physicianid,
                          isDeleted = t1.Isdeleted
                      };

            var provider = new ProviderViewModel
            {

                providerTableViewModels = phy,
                Region = region
            };

            return provider;
        }

        public ProviderViewModel FilterProvider(int RegionId)
        {
            if (RegionId == 0)
            {
                var region = from t1 in _db.Regions select t1;
                var phy = from t1 in _db.Physicians
                          join t2 in _db.Physiciannotifications on t1.Physicianid equals t2.Physicianid
                          join t3 in _db.Roles on t1.Roleid equals t3.Roleid
                          join t4 in _db.PhysicianStatuses on t1.Status equals t4.StatusId
                          select new ProviderTableViewModel
                          {
                              Firstname = t1.Firstname,
                              Lastname = t1.Lastname,
                              Email = t1.Email,
                              Mobile = t1.Mobile,
                              isNotiOff = t2.Isnotificationstopped,
                              Status = t4.StatusName,
                              Roleid = t3.Name,
                              Physicianid = t1.Physicianid
                          };
                var provider = new ProviderViewModel
                {
                    providerTableViewModels = phy,
                    Region = region
                };

                return provider;
            }
            else
            {
                var region = from t1 in _db.Regions select t1;
                var phy = from t1 in _db.Physicians
                          join t3 in _db.Physiciannotifications on t1.Physicianid equals t3.Physicianid
                          join t2 in _db.Physicianregions on t1.Physicianid equals t2.Physicianid
                          join t4 in _db.Roles on t1.Roleid equals t4.Roleid
                          join t5 in _db.PhysicianStatuses on t1.Status equals t5.StatusId
                          where t2.Regionid == RegionId
                          select new ProviderTableViewModel
                          {
                              Firstname = t1.Firstname,
                              Lastname = t1.Lastname,
                              Email = t1.Email,
                              Mobile = t1.Mobile,
                              isNotiOff = t3.Isnotificationstopped,
                              Status = t5.StatusName,
                              Roleid = t4.Name,
                              Physicianid = t1.Physicianid
                          };
                var provider = new ProviderViewModel
                {
                    providerTableViewModels = phy,
                    Region = region
                };

                return provider;
            }
        }

        public void StopNoty(int Physicianid)
        {
            var phy = _db.Physiciannotifications.Where(x => x.Physicianid == Physicianid).FirstOrDefault();
            if (phy != null)
            {
                if (phy.Isnotificationstopped == true)
                {
                    phy.Isnotificationstopped = false;
                }
                else
                {
                    phy.Isnotificationstopped = true;
                }

                _db.Physiciannotifications.Update(phy);
                _db.SaveChanges();
            }
            else
            {
                var phynoty = new Physiciannotification
                {
                    Physicianid = Physicianid,
                    Isnotificationstopped = false
                };

                _db.Physiciannotifications.Update(phynoty);
                _db.SaveChanges();
            }
            return;
        }

        public EditProvider EditProvider(int Physicianid)
        {
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == Physicianid);
            var aspRow = _db.AspNetUsers.FirstOrDefault(x => x.Id == phy.Aspnetuserid);
            var regionData = from t1 in _db.Regions
                             select new CheckBoxData
                             {
                                 Id = t1.Regionid,
                                 value = t1.Abbreviation,
                                 Checked = _db.Physicianregions.Any(x => x.Physicianid == phy.Physicianid && x.Regionid == t1.Regionid)
                             };
            var Role = _db.Roles.Where(x => x.Accounttype == 2);
            var status = from t1 in _db.PhysicianStatuses select t1;
            var data = new EditProvider
            {
                UserName = aspRow.UserName,
                Physicianid = Physicianid,
                Firstname = phy.Firstname,
                Lastname = phy.Lastname,
                Email = phy.Email,
                Status = phy.Status,
                Roleid = phy.Roleid,
                Mobile = phy.Mobile,
                Medicallicense = phy.Medicallicense,
                Npinumber = phy.Npinumber,
                Syncemailaddress = phy.Syncemailaddress,
                Address1 = phy.Address1,
                Address2 = phy.Address2,
                City = phy.City,
                Zip = phy.Zip,
                Altphone = phy.Altphone,
                Businessname = phy.Businessname,
                Businesswebsite = phy.Businesswebsite,
                Signature = phy.Signature,
                Photo = phy.Photo,
                Adminnotes = phy.Adminnotes,
                Region = regionData,
                Role = Role,
                Statuses = status
            };

            if (phy.Isagreementdoc == true)
            {
                data.Isagreementdoc = true;
                System.String? fileName = (from t1 in _db.ProviderFiles
                                           where t1.PhysicianId == phy.Physicianid && t1.FileType == 1
                                           select t1.FileName).FirstOrDefault();
                IFormFile formFile = new FormFile(System.IO.Stream.Null, 0, 0, "agreementdoc", fileName);
                data.agreementdoc = formFile;
            }
            if (phy.Isbackgrounddoc == true)
            {
                data.Isbackgrounddoc = true;
                System.String? fileName = (from t1 in _db.ProviderFiles
                                           where t1.PhysicianId == phy.Physicianid && t1.FileType == 2
                                           select t1.FileName).FirstOrDefault();
                IFormFile formFile = new FormFile(System.IO.Stream.Null, 0, 0, "backgrounddoc", fileName);
                data.backgrounddoc = formFile;
            }
            if (phy.Islicensedoc == true)
            {
                data.Islicensedoc = true;
                System.String? fileName = (from t1 in _db.ProviderFiles
                                           where t1.PhysicianId == phy.Physicianid && t1.FileType == 3
                                           select t1.FileName).FirstOrDefault();
                IFormFile formFile = new FormFile(System.IO.Stream.Null, 0, 0, "licensedoc", fileName);
                data.licensedoc = formFile;
            }
            if (phy.Isnondisclosuredoc == true)
            {
                data.Isnondisclosuredoc = true;
                System.String? fileName = (from t1 in _db.ProviderFiles
                                           where t1.PhysicianId == phy.Physicianid && t1.FileType == 4
                                           select t1.FileName).FirstOrDefault();
                IFormFile formFile = new FormFile(System.IO.Stream.Null, 0, 0, "nondisclosuredoc", fileName);
                data.nondisclosuredoc = formFile;
            }
            if (phy.Istrainingdoc == true)
            {
                data.Istrainingdoc = true;
                System.String? fileName = (from t1 in _db.ProviderFiles
                                           where t1.PhysicianId == phy.Physicianid && t1.FileType == 5
                                           select t1.FileName).FirstOrDefault();
                IFormFile formFile = new FormFile(System.IO.Stream.Null, 0, 0, "trainingdoc", fileName);
                data.trainingdoc = formFile;
            }

            return data;
        }

        public void ProviderAccountEdit(EditProvider obj)
        {
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == obj.Physicianid);

            phy.Status = obj.Status;
            phy.Roleid = obj.Roleid;
            _db.Physicians.Update(phy);
            _db.SaveChanges();

        }

        public void ProviderInfoEdit(EditProvider obj)
        {
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == obj.Physicianid);

            phy.Firstname = obj.Firstname;
            phy.Lastname = obj.Lastname;
            phy.Email = obj.Email;
            phy.Mobile = obj.Mobile;
            phy.Medicallicense = obj.Medicallicense;
            phy.Npinumber = obj.Npinumber;
            phy.Syncemailaddress = obj.Syncemailaddress;

            _db.Physicians.Update(phy);
            _db.SaveChanges();

        }
        public void ProviderMailingInfoEdit(EditProvider obj)
        {
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == obj.Physicianid);

            phy.Address1 = obj.Address1;
            phy.Address2 = obj.Address2;
            phy.City = obj.City;
            phy.Zip = obj.Zip;
            phy.Altphone = obj.Altphone;


            _db.Physicians.Update(phy);
            _db.SaveChanges();
        }
        public void ProviderProfileEdit(EditProvider obj)
        {
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == obj.Physicianid);

            phy.Businessname = obj.Businessname;
            phy.Businesswebsite = obj.Businesswebsite;
            if (obj.PhySign != null)
            {
                phy.Signature = obj.PhySign.FileName;
            }
            if (obj.PhyPhoto != null)
            {
                phy.Photo = obj.PhyPhoto.FileName;
            }
            phy.Adminnotes = obj.Adminnotes;

            _db.Physicians.Update(phy);
            _db.SaveChanges();

        }

        public void PhysicianRegionUpdate(List<CheckBoxData> selectedRegion, int Physicianid)
        {
            foreach (var item in selectedRegion)
            {
                var data = _db.Physicianregions.FirstOrDefault(x => x.Physicianid == Physicianid && x.Regionid == item.Id);

                if (data == null)
                {
                    if (item.Checked)
                    {
                        var phyRegion = new Physicianregion
                        {
                            Physicianid = Physicianid,
                            Regionid = item.Id,
                        };
                        _db.Physicianregions.Add(phyRegion);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    if (!item.Checked)
                    {
                        _db.Physicianregions.Remove(data);
                        _db.SaveChanges();
                    }
                }
            }
        }

        public void ResetPhysicianPass(string pass, int Physicianid)
        {
            var adminRow = _db.Physicians.Where(x => x.Physicianid == Physicianid).FirstOrDefault();
            var aspnetRow = _db.AspNetUsers.Where(x => x.Id == adminRow.Aspnetuserid).FirstOrDefault();

            aspnetRow.PasswordHash = pass;
            _db.AspNetUsers.Update(aspnetRow);
            _db.SaveChanges();
        }
        public void DeletePhysician(int Physicianid)
        {
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == Physicianid);
            phy.Isdeleted = true;
            _db.Physicians.Update(phy);
            _db.SaveChanges();
        }

        public EditProvider CreateProvider()
        {
            var region = from t1 in _db.Regions
                         select new CheckBoxData
                         {
                             Id = t1.Regionid,
                             value = t1.Name,
                         };
            var data = new EditProvider
            {
                Region = region,
                Role = _db.Roles.Where(x => x.Accounttype == 2)
            };

            return data;
        }

        public int CreateProvider(EditProvider obj, string pass, string AspId, IEnumerable<CheckBoxData> selectedRegion)
        {
            Guid guid = Guid.NewGuid();

            var asp = new AspNetUser
            {
                Id = guid.ToString(),
                UserName = "MD." + obj.Lastname + "." + obj.Firstname[0],
                Email = obj.Email,
                PasswordHash = pass,
                CreatedDate = DateTime.UtcNow,
            };
            _db.AspNetUsers.Add(asp);
            _db.SaveChanges();

            var provider = new Physician()
            {
                Aspnetuserid = guid.ToString(),
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Mobile = obj.Mobile,
                Medicallicense = obj.Medicallicense,
                Npinumber = obj.Npinumber,
                Address1 = obj.Address1,
                Address2 = obj.Address2,
                City = obj.City,
                Zip = obj.Zip,
                Altphone = obj.Altphone,
                Businessname = obj.Businessname,
                Businesswebsite = obj.Businesswebsite,
                Photo = obj.Photo,
                Adminnotes = obj.Adminnotes,
                Createddate = obj.Createddate,
                Createdby = AspId,
                Isagreementdoc = obj.Isagreementdoc,
                Isbackgrounddoc = obj.Isbackgrounddoc,
                Islicensedoc = obj.Islicensedoc,
                Isnondisclosuredoc = obj.Isnondisclosuredoc,
                Istrainingdoc = obj.Istrainingdoc,
                Roleid = obj.Roleid,
                Status = obj.Status
            };
            _db.Physicians.Add(provider);
            _db.SaveChanges();

            var phyNoty = new Physiciannotification()
            {
                Physicianid = provider.Physicianid,
                Isnotificationstopped = false
            };
            _db.Physiciannotifications.Add(phyNoty);
            _db.SaveChanges();

            foreach (var item in selectedRegion)
            {
                if (item.Checked)
                {
                    var phyRegion = new Physicianregion
                    {
                        Physicianid = provider.Physicianid,
                        Regionid = item.Id,
                    };
                    _db.Physicianregions.Add(phyRegion);
                    _db.SaveChanges();
                }
            }

            return provider.Physicianid;
        }
        public void UploadProviderFile(int physicianId, string filename, int fileType)
        {
            var data = _db.ProviderFiles.FirstOrDefault(x => x.PhysicianId == physicianId && x.FileType == fileType);
            if (data != null)
            {
                data.IsDeleted = true;
                _db.ProviderFiles.Update(data);
                _db.SaveChanges();
            }

            var providerFile = new ProviderFile
            {
                PhysicianId = physicianId,
                FileName = filename,
                FileType = fileType
            };
            _db.ProviderFiles.Add(providerFile);
            _db.SaveChanges();

        }

        public IEnumerable<Menu> PageListFilter(int id)
        {
            if (id == 0)
            {
                var data0 = from t1 in _db.Menus
                            select new Menu
                            {
                                Menuid = t1.Menuid,
                                Name = t1.Name,
                            };

                return data0;
            }

            var data = from t1 in _db.Menus
                       where t1.Accounttype == id
                       select new Menu
                       {
                           Menuid = t1.Menuid,
                           Name = t1.Name,
                       };

            return data;
        }

        public IEnumerable<CreateRole> CreateRole()
        {
            var data = from t1 in _db.Roles
                       join t2 in _db.AccountTypes on t1.Accounttype equals t2.Id
                       select new CreateRole
                       {
                           Roleid = t1.Roleid,
                           Name = t1.Name,
                           AccountType = t2.Name,
                           isdeleted = t1.Isdeleted
                       };

            return data;
        }
        public void CreateRole(IEnumerable<CheckBoxData> PageList, string AspId, int AccountType, string Name)
        {
            var role = new Role()
            {
                Name = Name,
                Accounttype = (short?)AccountType,
                Createdby = AspId,
                Createddate = DateTime.Now,
            };

            _db.Roles.Add(role);
            _db.SaveChanges();

            foreach (var item in PageList)
            {
                if (item.Checked)
                {
                    var rolemenu = new Rolemenu()
                    {
                        Roleid = role.Roleid,
                        Menuid = item.Id
                    };
                    _db.Rolemenus.Add(rolemenu);
                    _db.SaveChanges();
                }
            }
        }

        public CreateRole EditRole(int id)
        {
            var rolerow = _db.Roles.Where(x => x.Roleid == id).FirstOrDefault();
            var accType = _db.AccountTypes.Where(x => x.Id == rolerow.Accounttype).FirstOrDefault().Name;

            var select = from t1 in _db.Menus
                         where t1.Accounttype == rolerow.Accounttype
                         select new CheckBoxData
                         {
                             Id = (int)t1.Menuid,
                             value = t1.Name,
                             Checked = _db.Rolemenus.Where(x => x.Roleid == id && x.Menuid == t1.Menuid).Any()
                         };

            var data = new CreateRole()
            {
                Roleid = rolerow.Roleid,
                Name = rolerow.Name,
                AccountType = accType,
                accountTypes = _db.AccountTypes,
                Menu = _db.Menus,
                SelectedPage = select

            };

            return data;
        }

        public void EditRole(IEnumerable<CheckBoxData> PageList, string AspId, int AccountType, CreateRole obj)
        {
            var roleRow = _db.Roles.Where(x => x.Roleid == obj.Roleid).First();

            roleRow.Name = obj.Name;
            roleRow.Accounttype = (short?)AccountType;
            roleRow.Modifiedby = AspId;
            roleRow.Modifieddate = DateTime.Now;

            _db.Roles.Update(roleRow);

            foreach (var item in PageList)
            {
                var data = _db.Rolemenus.Where(x => x.Roleid == obj.Roleid && x.Menuid == item.Id).FirstOrDefault();
                if (data == null)
                {
                    if (item.Checked)
                    {
                        var roleMenu = new Rolemenu()
                        {
                            Menuid = item.Id,
                            Roleid = obj.Roleid
                        };
                        _db.Rolemenus.Add(roleMenu);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    if (!item.Checked)
                    {
                        _db.Rolemenus.Remove(data);
                        _db.SaveChanges();
                    }
                }
            }
        }

        public void DeleteRole(int RoleId)
        {
            var roleRow = _db.Roles.Where(x => x.Roleid == RoleId).FirstOrDefault();
            roleRow.Isdeleted = true;
            _db.Roles.Update(roleRow);
            _db.SaveChanges();
        }

        public CreateAdminViewModel CreateAdmin()
        {
            var role = (from t1 in _db.Roles where t1.Accounttype == 1 select t1);

            var data = new CreateAdminViewModel()
            {
                Regions = _db.Regions,
                Rolemenus = role
            };

            return data;
        }

        public void CreateAdmin(CreateAdminViewModel obj, string password, string AspId, IEnumerable<CheckBoxData> selectedRegion)
        {
            Guid guid = Guid.NewGuid();
            var asp = new AspNetUser()
            {
                UserName = obj.Lastname + obj.Firstname[0],
                PasswordHash = password,
                CreatedDate = DateTime.UtcNow,
                Email = obj.Email,
                Id = guid.ToString(),
                PhoneNumber = obj.Mobile,
            };
            _db.AspNetUsers.Add(asp);
            _db.SaveChanges();

            var admin = new Admin()
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Address1 = obj.Address1,
                Address2 = obj.Address2,
                Mobile = obj.Mobile,
                Altphone = obj.Altphone,
                City = obj.City,
                Aspnetuserid = guid.ToString(),
                Createdby = AspId,
                Createddate = DateTime.Now,
                Status = obj.Status,
                Zip = obj.Zip,
                Roleid = obj.Roleid 
            };
            _db.Admins.Add(admin);
            _db.SaveChanges();

            foreach (var item in selectedRegion)
            {
                if (item.Checked == true)
                {
                    var region = new Adminregion()
                    {
                        Adminid = admin.Adminid,
                        Regionid = item.Id
                    };
                    _db.Adminregions.Add(region);
                    _db.SaveChanges();
                }
            }
        }


        public CreateAdminViewModel EditAdmin(int id)
        {
            var admin = _db.Admins.FirstOrDefault(x => x.Adminid == id);
            var aspRow = _db.AspNetUsers.FirstOrDefault(x => x.Id == admin.Aspnetuserid);
            var regionData = from t1 in _db.Regions
                             select new CheckBoxData
                             {
                                 Id = t1.Regionid,
                                 value = t1.Abbreviation,
                                 Checked = _db.Adminregions.Any(x => x.Adminid == id && x.Regionid == t1.Regionid)
                             };
            var data = new CreateAdminViewModel
            {
                Username = aspRow.UserName,
                Adminid = id,
                Firstname = admin.Firstname,
                Lastname = admin.Lastname,
                Email = admin.Email,
                Status = admin.Status,
                Roleid = admin.Roleid,
                Mobile = admin.Mobile,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                Zip = admin.Zip,
                Altphone = admin.Altphone,
                Aspnetuserid = admin.Aspnetuserid,
                Isdeleted = admin.Isdeleted,
                CheckedRegion = regionData,
                Rolemenus = _db.Roles,
                Regions = _db.Regions
            };

            return data;
        }

        public void EditAdmin(CreateAdminViewModel obj, string AspId, IEnumerable<CheckBoxData> selectedRegion)
        {
            var asp = _db.AspNetUsers.First(x => x.Id == AspId);
            var admin = _db.Admins.FirstOrDefault(x => x.Adminid == obj.Adminid);

            admin.Firstname = obj.Firstname;
            admin.Lastname = obj.Lastname;
            admin.Address1 = obj.Address1;
            admin.Address2 = obj.Address2;
            admin.Mobile = obj.Mobile;
            admin.Altphone = obj.Altphone;
            admin.City = obj.City;
            admin.Zip = obj.Zip;
            admin.Status = obj.Status;
            admin.Email = obj.Email;
            admin.Roleid = obj.Roleid;

            _db.Admins.Update(admin);

            foreach (var item in selectedRegion)
            {
                var adminRegion = _db.Adminregions.FirstOrDefault(x => x.Adminid == obj.Adminid && x.Regionid == item.Id);

                if (adminRegion == null)
                {
                    if (item.Checked)
                    {

                        var region = new Adminregion()
                        {
                            Adminid = admin.Adminid,
                            Regionid = item.Id
                        };
                        _db.Adminregions.Add(region);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    if (!item.Checked)
                    {
                        _db.Adminregions.Remove(adminRegion);
                        _db.SaveChanges();
                    }
                }
            }
        }

        public UserAccessVM UserAccess()
        {
            var data = new UserAccessVM
            {
                Roles = _db.Roles,
            };
            return data;
        }

        public IQueryable<UserAccessTable> UserAccessTables(int accountType, int RoleId)
        {
            var data = new object();
            if (accountType == 2)
            {
                data = from t1 in _db.Physicians
                       join t2 in _db.Roles on t1.Roleid equals t2.Roleid
                       join t3 in _db.AccountTypes on t2.Accounttype equals t3.Id
                       join t4 in _db.PhysicianStatuses on t1.Status equals t4.StatusId into s
                       from T4 in s.DefaultIfEmpty()
                       where t2.Roleid == RoleId
                       select new UserAccessTable
                       {
                           userId = t1.Physicianid,
                           UserName = t1.Firstname + " " + t1.Lastname,
                           AccountType = t3.Name,
                           AccountTypeId = t3.Id,
                           Phone = t1.Mobile,
                           status = T4.StatusName ?? "",
                           openReq = _db.Requests.Where(x => x.Physicianid == t1.Physicianid).Count()
                       };
            }
            else if (accountType == 1)
            {
                data = from t2 in _db.Admins
                       join t3 in _db.Roles on t2.Roleid equals t3.Roleid
                       join t4 in _db.AccountTypes on t3.Accounttype equals t4.Id
                       join t5 in _db.PhysicianStatuses on t2.Status equals t5.StatusId into s
                       from T5 in s.DefaultIfEmpty()
                       where t3.Roleid == RoleId
                       select new UserAccessTable
                       {
                           userId = t2.Adminid,
                           UserName = t2.Firstname + " " + t2.Lastname,
                           AccountType = t4.Name,
                           AccountTypeId = t4.Id,
                           Phone = t2.Mobile,
                           status = T5.StatusName,
                            openReq = 0
                       };
            }
            else
            {
                data = (from t1 in _db.Physicians
                        join t2 in _db.Roles on t1.Roleid equals t2.Roleid
                        join t3 in _db.AccountTypes on t2.Accounttype equals t3.Id
                        join t4 in _db.PhysicianStatuses on t1.Status equals t4.StatusId into s
                        from T4 in s.DefaultIfEmpty()
                        select new UserAccessTable
                        {
                            userId = t1.Physicianid,
                            UserName = t1.Firstname + " " + t1.Lastname,
                            AccountType = t3.Name,
                            AccountTypeId = t3.Id,
                            Phone = t1.Mobile,
                            status = T4.StatusName,
                            openReq = _db.Requests.Where(x => x.Physicianid == t1.Physicianid).Count()

                        }).Union(from t2 in _db.Admins
                                 join t3 in _db.Roles on t2.Roleid equals t3.Roleid
                                 join t4 in _db.AccountTypes on t3.Accounttype equals t4.Id
                                 join t5 in _db.PhysicianStatuses on t2.Status equals t5.StatusId into s
                                 from T5 in s.DefaultIfEmpty()
                                 select new UserAccessTable
                                 {
                                     userId = t2.Adminid,
                                     UserName = t2.Firstname + " " + t2.Lastname,
                                     AccountType = t4.Name,
                                     AccountTypeId = t4.Id,
                                     Phone = t2.Mobile,
                                     status = T5.StatusName,
                                     openReq = 0
                                 }
                                          );
            }
            return (IQueryable<UserAccessTable>)data;
        }
    }
}