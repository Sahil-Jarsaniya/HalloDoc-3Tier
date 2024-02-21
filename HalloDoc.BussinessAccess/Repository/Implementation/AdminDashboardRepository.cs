using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
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

            var newReqData = from req in _db.Requests
                          join rc in _db.Requestclients on req.Requestid equals rc.Requestid 
                          join phy in _db.Physicians on req.Physicianid equals phy.Physicianid 
                          where req.Status == 1 
                          select new newReqViewModel
                          {
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
                              physicianName = phy.Firstname+ " "+phy.Lastname
                          };
            var pendingReqData = from req in _db.Requests
                             join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                             join phy in _db.Physicians on req.Physicianid equals phy.Physicianid
                             where req.Status == 2
                             select new pendingReqViewModel
                             {
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

    }
}
