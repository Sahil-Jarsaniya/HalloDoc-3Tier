using AspNetCore;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.utils;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.DataAccess.ViewModel.PhysicianDashboard;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class PhysicianSiteRepository : IPhysicianSiteRepository
    {
        private readonly ApplicationDbContext _db;
        public PhysicianSiteRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Region> GetRegion()
        {
            return _db.Regions.ToList();
        }
        public List<Admin> GetAdminList()
        {
            return _db.Admins.ToList();
        }

        public int GetPhysicianId(string AspId)
        {
            return _db.Physicians.FirstOrDefault(x => x.Aspnetuserid == AspId).Physicianid;
        }


        public bool PhysicianLocationUpdate(double latitude, double longitude, int phyId)
        {
            try
            {

                var phyLocation = _db.Physicianlocations.Where(x => x.Physicianid == phyId).FirstOrDefault();
                var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == phyId);
                if (phyLocation == null)
                {
                    var location = new Physicianlocation()
                    {
                        Physicianid = phyId,
                        Latitude = (decimal?)latitude,
                        Longtitude = (decimal?)longitude,
                        Createddate = DateTime.Now,
                        Physicianname = phy.Firstname + " " + phy.Lastname,
                    };
                    _db.Physicianlocations.Add(location);
                    _db.SaveChanges();
                }
                else
                {
                    phyLocation.Latitude = (decimal?)latitude;
                    phyLocation.Longtitude = (decimal?)longitude;
                    _db.Physicianlocations.Update(phyLocation);
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public countRequestViewModel DashboardCount(int phyId)
        {
            var newCount = (from t1 in _db.Requests
                            where t1.Status == (int)enumsFile.requestStatus.Assigned && t1.Physicianid == phyId
                            select t1
                            ).Count();
            var pendingCount = (from t1 in _db.Requests
                                where t1.Status == (int)enumsFile.requestStatus.Accepted && t1.Physicianid == phyId
                                select t1
                            ).Count();
            var activeCount = (from t1 in _db.Requests
                               where (t1.Status == (int)enumsFile.requestStatus.Consult || t1.Status == (int)enumsFile.requestStatus.MdOnHouseCall) && t1.Physicianid == phyId
                               select t1
                            ).Count();
            var concludeCount = (from t1 in _db.Requests
                                 where t1.Status == (int)enumsFile.requestStatus.Concluded && t1.Physicianid == phyId
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
                              where req.Status == (int)enumsFile.requestStatus.Assigned && req.Physicianid == phyId
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
                                 where req.Status == (int)enumsFile.requestStatus.Accepted && req.Physicianid == phyId
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
                                where (req.Status == (int)enumsFile.requestStatus.Consult || req.Status == (int)enumsFile.requestStatus.MdOnHouseCall) && req.Physicianid == phyId
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
                                    callType = req.Calltype ?? 0
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
                                  where req.Status == (int)enumsFile.requestStatus.Concluded && req.Physicianid == phyId
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
                    reqRow.Status = (int)enumsFile.requestStatus.Accepted;
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

        public bool Encounter(int reqClientId, string option)
        {
            try
            {
                var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
                var reqRow = _db.Requests.Where(x => x.Requestid == reqClientRow.Requestid).FirstOrDefault();
                if (option == "Consult")
                {
                    reqRow.Status = (int)enumsFile.requestStatus.Concluded;
                    reqRow.Calltype = (int)enumsFile.PhysicianCalltype.HouseCall;
                }
                else
                {
                    reqRow.Status = (int)enumsFile.requestStatus.MdOnHouseCall;
                    reqRow.Calltype = (int)enumsFile.PhysicianCalltype.Consult;
                }
                _db.Requests.Update(reqRow);
                _db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        public Encounter Encounter(int id)
        {
            Encounter encounter = _db.Encounters.FirstOrDefault(x => x.EncounterId == id);

            return encounter;
        }
        public bool HouseCallBtn(int id)
        {
            try
            {
                var reqClientRow = _db.Requestclients.Where(x => x.Requestclientid == id).FirstOrDefault();
                var reqRow = _db.Requests.Where(x => x.Requestid == reqClientRow.Requestid).FirstOrDefault();
                reqRow.Status = (int)enumsFile.requestStatus.Concluded;
                _db.Requests.Update(reqRow);
                _db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool FinalizeEncounter(int id)
        {
            try
            {
                var encounter = _db.Encounters.FirstOrDefault(x => x.EncounterId == id);
                encounter.Isfinalized = true;
                encounter.FinalizedDate = DateTime.Now;
                _db.Encounters.Update(encounter);
                _db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool TransferCase(int reqClientId, string note)
        {
            try
            {
                var reqClientRow = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
                var reqRow = _db.Requests.FirstOrDefault(x => x.Requestid == reqClientRow.Requestid);
                var reqstatuslog = new Requeststatuslog()
                {
                    Requestid = reqRow.Requestid,
                    Status = 1,
                    Physicianid = reqRow.Physicianid,
                    Notes = note,
                    Createddate = DateTime.Now,
                    Transtoadmin = true,
                };
                reqRow.Status = (int)enumsFile.requestStatus.Unassigned;
                reqRow.Physicianid = null;
                _db.Requeststatuslogs.Add(reqstatuslog); _db.SaveChanges();
                _db.Requests.Update(reqRow);
                _db.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public void ViewNotePost(int reqClientId, string Note, int phyId, string phyAspId)
        {
            var reqIdCol = _db.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            var reqId = reqIdCol.Requestid;
            var Status = _db.Requests.Where(x => x.Requestid == reqId).FirstOrDefault().Status;

            var reqNote = _db.Requestnotes.Where(x => x.Requestid == reqId).FirstOrDefault();

            if (reqNote == null)
            {
                var reqNoteDb = new Requestnote
                {
                    Requestid = (int)reqId,
                    Physiciannotes = Note,
                    Createddate = DateTime.Now,
                    Createdby = phyAspId
                };
                _db.Requestnotes.Add(reqNoteDb);
                _db.SaveChanges();
            }
            else
            {
                var reqNoteDb = _db.Requestnotes.Where(x => x.Requestid == reqId).FirstOrDefault();

                reqNoteDb.Requestid = (int)reqId;
                reqNoteDb.Physiciannotes = Note;
                reqNoteDb.Modifieddate = DateTime.Now;
                reqNoteDb.Modifiedby = phyAspId;
                _db.Requestnotes.Update(reqNoteDb);
                _db.SaveChanges();
            }

            var reqStatusLog = new Requeststatuslog
            {
                Requestid = (int)reqId,
                Status = Status,
                Physicianid = phyId,
                Notes = Note,
                Createddate = DateTime.Now,
            };
            _db.Requeststatuslogs.Add(reqStatusLog);
            _db.SaveChanges();
        }

        public void ConcludeCare(CloseCaseViewModel obj, string aspId)
        {
            var reqCRow = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == obj.ReqClientId);
            var reqRow = _db.Requests.FirstOrDefault(x => x.Requestid == reqCRow.Requestid);

            reqRow.Status = 5;
            _db.Requests.Update(reqRow);
            _db.SaveChanges();
            var reqNote = _db.Requestnotes.Where(x => x.Requestid == reqRow.Requestid).FirstOrDefault();

            if (reqNote == null)
            {
                var reqNoteDb = new Requestnote
                {
                    Requestid = reqRow.Requestid,
                    Physiciannotes = obj.note,
                    Createddate = DateTime.Now,
                    Createdby = aspId
                };
                _db.Requestnotes.Add(reqNoteDb);
                _db.SaveChanges();
            }
            else
            {

                var reqNoteDb = _db.Requestnotes.Where(x => x.Requestid == reqRow.Requestid).FirstOrDefault();

                reqNoteDb.Requestid = reqRow.Requestid;
                reqNoteDb.Physiciannotes = obj.note;
                reqNoteDb.Modifieddate = DateTime.Now;
                reqNoteDb.Modifiedby = aspId;
                _db.Requestnotes.Update(reqNoteDb);
                _db.SaveChanges();
            }

            var reqStatusLog = new Requeststatuslog
            {
                Requestid = reqRow.Requestid,
                Status = reqRow.Status,
                Physicianid = reqRow.Physicianid,
                Notes = obj.note,
                Createddate = DateTime.Now,
            };
            _db.Requeststatuslogs.Add(reqStatusLog);
            _db.SaveChanges();
        }

        public void viewUplodPost(string file, int reqId, int phyId)
        {
            Requestwisefile requestwisefile = new Requestwisefile
            {
                Filename = file,
                Requestid = reqId,
                Createddate = DateTime.Now,
                Physicianid = phyId
            };

            _db.Requestwisefiles.Add(requestwisefile);
            _db.SaveChanges();
        }

        public List<sheetData> sheetData(string date, int phyId)
        {
            var startDate = DateOnly.ParseExact(date, "d/M/yyyy");
            var day = startDate.Day;
            var month = startDate.Month;
            var year = startDate.Year;
            var startDay = 0;
            var endDay = 0;
            if (day <= 14)
            {
                startDay = 1;
                endDay = 14;
            }
            else
            {
                startDay = 15;
                endDay = DateTime.DaysInMonth(year, month);
            }
            var endDate = startDate.AddDays(endDay - startDay);

            var data = (from t1 in _db.BiWeeklySheets
                        where t1.Date >= startDate && t1.Date <= endDate && t1.Physicianid == phyId
                        select new sheetData()
                        {
                            HouseCall = t1.NumberOfHousecall ?? 0,
                            shift = t1.Totalhour.Value.Hours,
                            Date = t1.Date,
                            phoneConsult = t1.NumberOfPhoneConsult ?? 0
                        }).ToList();

            return data;
        }

        public IQueryable<BiWeeklyRecieptVM> ReceiptData(string date, int phyId)
        {
            var startDate = DateOnly.ParseExact(date, "d/M/yyyy");
            var day = startDate.Day;
            var month = startDate.Month;
            var year = startDate.Year;
            var startDay = 0;
            var endDay = 0;
            if (day <= 14)
            {
                startDay = 1;
                endDay = 14;
            }
            else
            {
                startDay = 15;
                endDay = DateTime.DaysInMonth(year, month);
            }
            var endDate = startDate.AddDays(endDay - startDay);

            var data = from t1 in _db.BiWeeklyReceipts
                       where t1.Date >= startDate && t1.Date <= endDate && t1.Physicianid == phyId
                       select new BiWeeklyRecieptVM()
                       {
                           Date = (DateOnly)t1.Date,
                           item = t1.Item ?? "",
                           billName = t1.Bill,
                           amount = t1.Amount ?? 0,
                           PhysicianId = phyId,
                       };
            return data;
        }

        public bool isFinalizedSheet(string date, int phyId)
        {
            var startDate = DateOnly.ParseExact(date, "d/M/yyyy");
            var day = startDate.Day;
            var month = startDate.Month;
            var year = startDate.Year;
            var startDay = 0;
            var endDay = 0;
            if (day <= 14)
            {
                startDay = 1;
                endDay = 14;
            }
            else
            {
                startDay = 15;
                endDay = DateTime.DaysInMonth(year, month);
            }
            var endDate = startDate.AddDays(endDay - startDay);

            var sheet = _db.TimeSheets.FirstOrDefault(x => x.StartDate == startDate && x.EndDate == endDate && x.PhysicianId == phyId);

            return sheet.IsFinal ?? false;
        }

        public DateVM biweeklySheetVMs(string date, int phyId)
        {
            var startDate = DateOnly.ParseExact(date, "d/M/yyyy");
            var day = startDate.Day;
            var month = startDate.Month;
            var year = startDate.Year;
            var startDay = 0;
            var endDay = 0;
            if (day <= 14)
            {
                startDay = 1;
                endDay = 14;
            }
            else
            {
                startDay = 15;
                endDay = DateTime.DaysInMonth(year, month);
            }
            var endDate = startDate.AddDays(endDay - startDay);



            var data = (from t1 in _db.Shiftdetails
                        join t2 in _db.Shifts on t1.Shiftid equals t2.Shiftid
                        where t1.Shiftdate >= startDate && t1.Shiftdate <= endDate && t2.Physicianid == phyId
                        select new BiweeklySheetVM()
                        {
                            Date = t1.Shiftdate,
                            OnCallStatus = t1.Endtime - t1.Starttime,
                            TotalHour = t1.Endtime - t1.Starttime
                        }).OrderBy(x => x.Date).GroupBy(x => x.Date);

            List<BiweeklySheetVM> list = new List<BiweeklySheetVM>();

            for (int i = 0; i <= endDay - startDay; i++)
            {
                var currentDate = startDate.AddDays(i);
                var totalHour = System.TimeSpan.Zero;
                foreach (var group in data)
                {
                    if (group.Any(x => x.Date == currentDate))
                    {
                        foreach (var item in group)
                        {
                            totalHour += item.OnCallStatus;
                        }
                    }
                }
                var existObj = _db.BiWeeklySheets.FirstOrDefault(x => x.Date == currentDate && x.Physicianid == phyId);

                if (existObj != null)
                {
                    var obj = new BiweeklySheetVM()
                    {
                        Date = currentDate,
                        OnCallStatus = totalHour,
                        TotalHour = existObj.Totalhour ?? TimeSpan.MinValue,
                        Weekend = existObj.Weekend ?? false,
                        NumberOfHouseCall = existObj.NumberOfHousecall ?? 0,
                        NumberOfPhoneConsults = existObj.NumberOfPhoneConsult ?? 0
                    };
                    list.Add(obj);
                }
                else
                {
                    var obj = new BiweeklySheetVM()
                    {
                        Date = currentDate,
                        OnCallStatus = totalHour,
                        TotalHour = totalHour,
                    };
                    list.Add(obj);
                }
            }

            List<BiWeeklyRecieptVM> listofReciept = new List<BiWeeklyRecieptVM>();
            for (int i = 0; i <= endDay - startDay; i++)
            {
                var currentDate = startDate.AddDays(i);

                var existObj = _db.BiWeeklyReceipts.FirstOrDefault(x => x.Date == currentDate && x.Physicianid == phyId);

                if (existObj != null)
                {

                    var obj = new BiWeeklyRecieptVM()
                    {
                        Date = currentDate,
                        item = existObj.Item ?? "",
                        amount = existObj.Amount ?? 0,
                        billName = existObj.Bill,
                        isUploaded = true,
                        PhysicianId = phyId
                    };
                    listofReciept.Add(obj);
                }
                else
                {
                    var obj = new BiWeeklyRecieptVM()
                    {
                        Date = currentDate,
                        item = "",
                        amount = 0,
                        isUploaded = false,
                        PhysicianId = phyId
                    };
                    listofReciept.Add(obj);
                }
            }

            var data1 = new DateVM()
            {
                StartDate = startDate,
                EndDate = endDate,
                isFinal = false,
                biweeklySheetVMs = list,
                biWeeklyReciepts = listofReciept
            };
            return data1;
        }

        public void biweeklySheetVMs(DateVM obj, int phyId)
        {
            var Sheet = _db.TimeSheets.Where(x => x.StartDate == obj.StartDate && x.EndDate == obj.EndDate && x.PhysicianId == phyId).FirstOrDefault();

            if (Sheet != null)
            {
                foreach (var sheet in obj.biweeklySheetVMs)
                {
                    var biweeklySheet = _db.BiWeeklySheets.Where(x => x.Date == sheet.Date && x.Physicianid == phyId).FirstOrDefault();
                    biweeklySheet.Date = sheet.Date;
                    biweeklySheet.Physicianid = phyId;
                    biweeklySheet.Weekend = sheet.Weekend;
                    biweeklySheet.Totalhour = sheet.TotalHour;
                    biweeklySheet.OnCallStatus = sheet.OnCallStatus;
                    biweeklySheet.NumberOfHousecall = sheet.NumberOfHouseCall;
                    biweeklySheet.NumberOfPhoneConsult = sheet.NumberOfPhoneConsults;

                    _db.BiWeeklySheets.Update(biweeklySheet);
                    _db.SaveChanges();
                }
                Sheet.IsFinal = obj.isFinal;
                Sheet.Status = "Pending";
                _db.TimeSheets.Update(Sheet);
                _db.SaveChanges();
            }
            else
            {
                foreach (var sheet in obj.biweeklySheetVMs)
                {
                    var biweeklySheet = new BiWeeklySheet()
                    {
                        Date = sheet.Date,
                        Physicianid = phyId,
                        Weekend = sheet.Weekend,
                        Totalhour = sheet.TotalHour,
                        OnCallStatus = sheet.OnCallStatus,
                        NumberOfHousecall = sheet.NumberOfHouseCall,
                        NumberOfPhoneConsult = sheet.NumberOfPhoneConsults
                    };
                    _db.BiWeeklySheets.Add(biweeklySheet);
                    _db.SaveChanges();
                }

                var TimeSheet = new TimeSheet()
                {
                    StartDate = obj.StartDate,
                    EndDate = obj.EndDate,
                    IsSheetCreated = true,
                    PhysicianId = phyId,
                    IsFinal = obj.isFinal,
                    Status = "Pending"
                };
                _db.TimeSheets.Add(TimeSheet);
                _db.SaveChanges();
            }
        }


        public void BiWeeklyReciept(BiWeeklyRecieptVM obj, int phyId)
        {
            var existData = _db.BiWeeklyReceipts.FirstOrDefault(x => x.Date == obj.Date && x.Physicianid == phyId);

            if (existData != null)
            {
                existData.Item = obj.item;
                existData.Amount = obj.amount;
                existData.Bill = obj.bill.FileName.ToString();
                existData.Physicianid = phyId;
                _db.BiWeeklyReceipts.Update(existData);
                _db.SaveChanges();
            }
            else
            {
                var data = new BiWeeklyReceipt()
                {
                    Date = obj.Date,
                    Item = obj.item,
                    Amount = obj.amount,
                    Bill = obj.bill.FileName.ToString(),
                    Physicianid = phyId
                };
                _db.BiWeeklyReceipts.Add(data);
                _db.SaveChanges();
            }
        }
        public void DeleteBill(string date, int phyId)
        {
            var startDate = DateOnly.ParseExact(date, "M/d/yyyy");
            var data = _db.BiWeeklyReceipts.FirstOrDefault(x => x.Date == startDate && x.Physicianid == phyId);

            _db.BiWeeklyReceipts.Remove(data);
            _db.SaveChanges();
        }

        public Chat ChatWithAdmin(int reqClientId, int phyId)
        {
            var history = (from t1 in _db.PhysicianChats
                           join t2 in _db.Physicians on t1.PhysicianId equals t2.Physicianid
                           where t1.ReqClientId == reqClientId && t1.PhysicianId == phyId && t1.SenderAccountType == (int)enumsFile.AccountType.Admin
                           select new ChatHistory()
                           {
                               Message = t1.Message,
                               CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                               CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                               Sender = t2.Firstname + " " + t2.Lastname,
                               isMyMsg = true
                           });
            var adminHistory = (from t1 in _db.AdminChats
                                join t2 in _db.Admins on t1.AdminId equals t2.Adminid
                                where t1.ReqClientId == reqClientId && t1.SenderAccountType == (int)enumsFile.AccountType.Physician
                                select new ChatHistory()
                                {
                                    Message = t1.Message,
                                    CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                                    CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                                    Sender = t2.Firstname + " " + t2.Lastname,
                                    isMyMsg = false
                                });
            var list = history.Union(adminHistory);
            list = list.OrderBy(x => x.CreatedOn).ThenBy(x => x.CreatedAt);

            var phyRow = _db.Physicians.FirstOrDefault(x => x.Physicianid == phyId);
            var data = new Chat()
            {
                Sender = phyRow.Firstname + " " + phyRow.Lastname,
                SenderId = phyId,
                reqClientId = reqClientId,
                chatHistories = list.ToList(),
                AccountTypeOfSender = (int)enumsFile.AccountType.Physician,
                AccountTypeOfReceiver = (int)enumsFile.AccountType.Admin
            };
            return data;
        }

        public Chat ChatWithPatient(int reqClientId, int phyId)
        {
            var history = (from t1 in _db.PhysicianChats
                           join t2 in _db.Physicians on t1.PhysicianId equals t2.Physicianid
                           where t1.ReqClientId == reqClientId && t1.PhysicianId == phyId && t1.SenderAccountType == (int)enumsFile.AccountType.Patient
                           select new ChatHistory()
                           {
                               Message = t1.Message,
                               CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                               CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                               Sender = t2.Firstname + " " + t2.Lastname,
                               isMyMsg = true
                           });

            var patientHistory = (from t1 in _db.PatientChats
                                join t2 in _db.Requestclients on t1.ReqClientId equals t2.Requestclientid
                                where t1.ReqClientId == reqClientId && t1.SenderAccountType == (int)enumsFile.AccountType.Physician
                                select new ChatHistory()
                                {
                                    Message = t1.Message,
                                    CreatedAt = TimeOnly.FromDateTime(t1.CreateTime),
                                    CreatedOn = DateOnly.FromDateTime(t1.CreateTime),
                                    Sender = t2.Firstname + " " + t2.Lastname,
                                    isMyMsg = false
                                });
            var list = history.Union(patientHistory);
            list = list.OrderBy(x => x.CreatedOn).ThenBy(x => x.CreatedAt);
            var phyRow = _db.Physicians.FirstOrDefault(x => x.Physicianid == phyId);
            var data = new Chat()
            {
                Sender = phyRow.Firstname + " " + phyRow.Lastname,
                SenderId = phyId,
                reqClientId = reqClientId,
                chatHistories = list.ToList(),
                AccountTypeOfSender = (int)enumsFile.AccountType.Physician,
                AccountTypeOfReceiver = (int)enumsFile.AccountType.Patient
            };
            return data;
        }
        public void StoreChat(int reqClientId, int phyId, string message, int AccountTypeOfReceiver)
        {
            var chat = new PhysicianChat()
            {
                PhysicianId = phyId,
                ReqClientId = reqClientId,
                CreateTime = DateTime.UtcNow,
                Message = message,
                SenderAccountType = AccountTypeOfReceiver,
            };
            _db.PhysicianChats.Add(chat);
            _db.SaveChanges();
        }
    }
}
