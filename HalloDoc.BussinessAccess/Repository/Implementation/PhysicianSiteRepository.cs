using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class PhysicianSiteRepository : IPhysicianSiteRepository
    {
        private readonly ApplicationDbContext _db;

        public PhysicianSiteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public int GetPhysicianId(string AspId)
        {
            return _db.Physicians.FirstOrDefault(x => x.Aspnetuserid == AspId).Physicianid;
        }


        public countRequestViewModel DashboardCount(int phyId)
        {
            var newCount = (from t1 in _db.Requests
                            where t1.Status == 1 && t1.Physicianid == phyId
                            select t1
                            ).Count();
            var pendingCount = (from t1 in _db.Requests
                                where t1.Status == 2 && t1.Physicianid == phyId
                                select t1
                            ).Count();
            var activeCount = (from t1 in _db.Requests
                               where t1.Status == 8 || t1.Status == 15 && t1.Physicianid == phyId
                               select t1
                            ).Count();
            var concludeCount = (from t1 in _db.Requests
                                 where t1.Status == 4 && t1.Physicianid == phyId
                                 select t1
                            ).Count();
            var count = new countRequestViewModel
            {
                newCount = newCount,
                pendingCount = pendingCount,
                activeCount = activeCount,
                concludeCount = concludeCount
            };

            return count;
        }
        public IQueryable<pendingReqViewModel> newReq(searchViewModel? obj, int phyId)
        {
            var newReqData = (from req in _db.Requests
                              join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                              where req.Status == 1 && req.Physicianid == phyId
                              select new pendingReqViewModel
                              {
                                  reqClientId = rc.Requestclientid,
                                  Firstname = rc.Firstname,
                                  Lastname = rc.Lastname,
                                  Strmonth = rc.Strmonth,
                                  Createddate = req.Createddate,
                                  Phonenumber = rc.Phonenumber,
                                  reqTypeId = req.Requesttypeid,
                                  Regionid = rc.Regionid,
                                  Email = rc.Email,
                                  Status = req.Status,
                                  Street = rc.Street,
                                  State = rc.State,
                                  Zipcode = rc.Zipcode,
                                  City = rc.City,
                              });
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                newReqData = newReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name));
            }
            if (obj.reqType != 0)
            {
                newReqData = newReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return newReqData;
        }

        public IQueryable<pendingReqViewModel> pendingReq(searchViewModel? obj, int phyId)
        {
            var pendingReqData = from req in _db.Requests
                                 join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                                 where req.Status == 2 && req.Physicianid == phyId
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
                                     reqTypeId = req.Requesttypeid,
                                     Email = rc.Email,
                                     Status = req.Status,
                                     Street = rc.Street,
                                     State = rc.State,
                                     Zipcode = rc.Zipcode,
                                     City = rc.City,
                                 };
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                pendingReqData = pendingReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name));
            }
            if (obj.reqType != 0)
            {
                pendingReqData = pendingReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return pendingReqData;
        }

        public IQueryable<pendingReqViewModel> activeReq(searchViewModel? obj, int phyId)
        {
            var activeReqData = from req in _db.Requests
                                join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                                where (req.Status == 8 || req.Status == 15) && req.Physicianid == phyId
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
                                    reqTypeId = req.Requesttypeid,
                                    Email = rc.Email,
                                    Status = req.Status,
                                    Street = rc.Street,
                                    State = rc.State,
                                    Zipcode = rc.Zipcode,
                                    City = rc.City,
                                };
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                activeReqData = activeReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name));
            }
            if (obj.reqType != 0)
            {
                activeReqData = activeReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return activeReqData;
        }

        public IQueryable<pendingReqViewModel> concludeReq(searchViewModel? obj, int phyId)
        {
            var concludeReqData = from req in _db.Requests
                                  join rc in _db.Requestclients on req.Requestid equals rc.Requestid
                                  where req.Status == 4 && req.Physicianid == phyId
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
                                      reqTypeId = req.Requesttypeid,
                                      Email = rc.Email,
                                      Status = req.Status,
                                      Street = rc.Street,
                                      State = rc.State,
                                      Zipcode = rc.Zipcode,
                                      City = rc.City,
                                  };
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                concludeReqData = concludeReqData.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name));
            }
            if (obj.reqType != 0)
            {
                concludeReqData = concludeReqData.Where(s => s.reqTypeId == obj.reqType);
            }
            return concludeReqData;
        }

        public MonthScheduling monthScheduling(string date, int phyId)
        {
            var date1 = DateOnly.Parse(date);
            var day = from t1 in _db.Physicians.Where(x => x.Physicianid == phyId)
                      join t2 in _db.Shifts
                      on t1.Physicianid equals t2.Physicianid into physicianShifts
                      from t2 in physicianShifts.DefaultIfEmpty()
                      join t3 in _db.Shiftdetails
                      .Where(x => x.Shiftdate.Month == date1.Month && x.Shiftdate.Year == date1.Year && x.Isdeleted != true)
                      on t2.Shiftid equals t3.Shiftid into shiftDetails
                      from t3 in shiftDetails.DefaultIfEmpty()
                      select new DayScheduling()
                      {
                          PhysicianId = t1.Physicianid,
                          PhysicianName = t1.Firstname + " " + t1.Lastname,
                          Shiftid = t2 != null ? t2.Shiftid : null,
                          shiftDetailId = t3 != null ? t3.Shiftdetailid : null,
                          Startdate = t3 != null ? t3.Shiftdate : null,
                          EndTime = t3 != null ? t3.Endtime : null,
                          StartTime = t3 != null ? t3.Starttime : null,
                          SelectedDate = date1,
                          ShiftDate = t3 != null ? t3.Shiftdate : null,
                          status = t3 != null ? t3.Status : null,
                          regionId = t3.Regionid
                      };

            var data = new MonthScheduling
            {
                physicians = _db.Physicians,
                Selecteddate = date1,
                DaySchedulings = day,
            };

            return data;
        }

        public List<Region> CreateShiftRegion(int phyId)
        {
            return (from t1 in _db.Regions
                    join t2 in _db.Physicianregions on t1.Regionid equals t2.Regionid
                    where t2.Physicianid == phyId
                    select t1).ToList();
        }

        public IEnumerable<DayScheduling> ViewAllShift(string date, int phyId)
        {
            var date1 = DateOnly.Parse(date);
            var data = from t3 in _db.Shiftdetails.Where(x => x.Shiftdate.Month == date1.Month && x.Shiftdate.Day == date1.Day && x.Shiftdate.Year == date1.Year && x.Isdeleted != true)
                       join t2 in _db.Shifts on t3.Shiftid equals t2.Shiftid
                       join t1 in _db.Physicians.Where(x => x.Physicianid == phyId)
                       on t2.Physicianid equals t1.Physicianid
                       select new DayScheduling
                       {
                           PhysicianId = t1.Physicianid,
                           PhysicianName = t1.Firstname + " " + t1.Lastname,
                           Shiftid = t2 != null ? t2.Shiftid : null,
                           shiftDetailId = t3 != null ? t3.Shiftdetailid : null,
                           Startdate = t2 != null ? t2.Startdate : null,
                           EndTime = t3 != null ? t3.Endtime : null,
                           StartTime = t3 != null ? t3.Starttime : null,
                           SelectedDate = date1,
                           ShiftDate = t3 != null ? t3.Shiftdate : null,
                           status = t3 != null ? t3.Status : null,
                       };

            return data;
        }

        public bool AccpetRequest(int reqClientId)
        {
            var reqClientRow = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            var reqRow = _db.Requests.FirstOrDefault(x => x.Requestid == reqClientRow.Requestid);
            if (reqRow != null)
            {
                try
                {
                    reqRow.Status = 2;
                    _db.Requests.Update(reqRow);
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
