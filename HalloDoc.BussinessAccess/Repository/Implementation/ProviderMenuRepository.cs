using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class ProviderMenuRepository : IProviderMenuRepository
    {
        private readonly ApplicationDbContext _db;

        public ProviderMenuRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Region> Regions()
        {
            return _db.Regions;
        }

        public void CreateShift(string selectedDays, CreateShift obj, string AspId)
        {
            var day = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedDays);
            var curDate = obj.Startdate;
            var curDay = (int)obj.Startdate.DayOfWeek;

            var shift = new Shift()
            {
                Physicianid = obj.Physicianid,
                Startdate = obj.Startdate,
                Isrepeat = obj.Isrepeat,
                Repeatupto = obj.Repeatupto,
                Createdby = AspId,
                Createddate = DateTime.Now,
            };
            _db.Shifts.Add(shift);
            _db.SaveChanges();
            if (obj.Isrepeat == false)
            {
                var shiftdetail = new Shiftdetail()
                {
                    Shiftid = shift.Shiftid,
                    Shiftdate = obj.Startdate,
                    Starttime = obj.StartTime,
                    Endtime = obj.EndTime,
                    Regionid = obj.Regionid,

                };
                _db.Shiftdetails.Add(shiftdetail);
                _db.SaveChanges();

                var shiftRegion = new Shiftdetailregion()
                {
                    Regionid = obj.Regionid,
                    Shiftdetailid = shiftdetail.Shiftdetailid,
                };
                _db.Shiftdetailregions.Add(shiftRegion);
                _db.SaveChanges();
            }
            else
            {
                for (int i = 1; i <= obj.Repeatupto; i++)
                {
                    foreach (var item in day)
                    {
                        if (item.Checked)
                        {
                            var shiftDay = (7 - curDay + item.Id) % 7 + 7 * (i - 1);
                            var shiftDate = curDate.AddDays(shiftDay);
                            var shiftdetail = new Shiftdetail()
                            {
                                Shiftid = shift.Shiftid,
                                Shiftdate = shiftDate,
                                Starttime = obj.StartTime,
                                Endtime = obj.EndTime,

                            };
                            _db.Shiftdetails.Add(shiftdetail);
                            _db.SaveChanges();

                            var shiftRegion = new Shiftdetailregion()
                            {
                                Regionid = obj.Regionid,
                                Shiftdetailid = shiftdetail.Shiftdetailid,
                            };
                            _db.Shiftdetailregions.Add(shiftRegion);
                            _db.SaveChanges();
                        }
                    }
                }
            }
        }

        public IEnumerable<DayScheduling> DayWiseScheduling(string date)
        {
            var date1 = DateOnly.Parse(date);

            var data = (from t1 in _db.Physicians
                        join t2 in _db.Shifts
                        on t1.Physicianid equals t2.Physicianid into physicianShifts
                        from t2 in physicianShifts.DefaultIfEmpty()
                        join t3 in _db.Shiftdetails
                        .Where(x => x.Shiftdate.Month == date1.Month && x.Shiftdate.Day == date1.Day && x.Shiftdate.Year == date1.Year && x.Isdeleted != true)
                        on t2.Shiftid equals t3.Shiftid into shiftDetails
                        from t3 in shiftDetails.DefaultIfEmpty()
                        select new DayScheduling()
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
                            regionId = t3.Regionid
                        }).OrderBy(d => d.PhysicianId);
            return data;
        }

        public WeekScheduling WeekWiseScheduling(string date)
        {
            var date1 = DateOnly.Parse(date);
            var day = from t1 in _db.Physicians
                      join t2 in _db.Shifts
                      on t1.Physicianid equals t2.Physicianid into physicianShifts
                      from t2 in physicianShifts.DefaultIfEmpty()
                      join t3 in _db.Shiftdetails
                      .Where(x => x.Shiftdate >= date1 && x.Shiftdate <= date1.AddDays(7) && x.Isdeleted != true)
                      on t2.Shiftid equals t3.Shiftid into shiftDetails
                      from t3 in shiftDetails.DefaultIfEmpty()
                      select new DayScheduling()
                      {
                          PhysicianId = t1.Physicianid,
                          PhysicianName = t1.Firstname + " " + t1.Lastname,
                          Shiftid = t2 != null ? t2.Shiftid : null,
                          shiftDetailId = t3 != null ? t3.Shiftdetailid : null,
                          Startdate = t3 != null ? t3.Shiftdate: null,
                          EndTime = t3 != null ? t3.Endtime : null,
                          StartTime = t3 != null ? t3.Starttime : null,
                          SelectedDate = date1,
                          ShiftDate = t3 != null ? t3.Shiftdate : null,
                          status = t3 != null ? t3.Status : null,
                          regionId = t3.Regionid
                      };
            var data = new WeekScheduling()
            {
                physicians = _db.Physicians,
                Selecteddate = date1,
                daySchedulings = day
            };

            return data;
        }

        public MonthScheduling MonthScheduling(string date)
        {
            var date1 = DateOnly.Parse(date);
            var day = from t1 in _db.Physicians
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

        public CreateShift ViewShift(int shiftDetailId)
        {
            var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftDetailId);
            var shift = _db.Shifts.FirstOrDefault(x => x.Shiftid == shiftDetail.Shiftid);
            var shiftReg = _db.Shiftdetailregions.FirstOrDefault(x => x.Shiftdetailid == shiftDetail.Shiftdetailid);
            var phy = _db.Physicians.FirstOrDefault(x => x.Physicianid == shift.Physicianid);

            var data = new CreateShift()
            {
                Physicianid = phy.Physicianid,
                PhysicianName = phy.Firstname + " " + phy.Lastname,
                Regionid = shiftReg.Regionid,
                Startdate = shiftDetail.Shiftdate,
                StartTime = shiftDetail.Starttime,
                EndTime = shiftDetail.Endtime,
                Shiftid = shiftDetail.Shiftid,
                Regions = _db.Regions,
                ShiftDetailId = shiftDetail.Shiftdetailid,
                status = shiftDetail.Status,
            };

            return data;
        }
        public IEnumerable<DayScheduling> ViewAllShift(string date)
        {
            var date1 = DateOnly.Parse(date);
            var data = from t3 in _db.Shiftdetails.Where(x => x.Shiftdate.Month == date1.Month && x.Shiftdate.Day == date1.Day && x.Shiftdate.Year == date1.Year && x.Isdeleted != true)
                       join t2 in _db.Shifts on t3.Shiftid equals t2.Shiftid
                       join t1 in _db.Physicians
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
        public bool ReturnShift(int shiftDetailId)
        {
           
            try
            {
                var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftDetailId);
                shiftDetail.Status = 2;
                _db.Shiftdetails.Update(shiftDetail);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteShift(int shiftDetailId)
        {
            try
            {
                var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftDetailId);
                shiftDetail.Isdeleted = true;
                _db.Shiftdetails.Update(shiftDetail);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateShift(CreateShift obj, int id)
        {
            try
            {
                var shiftDetail = _db.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == id);
                    shiftDetail.Shiftdate = obj.Startdate;
                    shiftDetail.Endtime = obj.EndTime;
                    shiftDetail.Starttime = obj.StartTime;
                    shiftDetail.Modifieddate = DateTime.Now;
                    shiftDetail.Regionid = obj.Regionid;
                    _db.Shiftdetails.Update(shiftDetail);

                var shift = _db.Shifts.FirstOrDefault(x => x.Shiftid == shiftDetail.Shiftid);
                shift.Physicianid = obj.Physicianid;
                _db.Shifts.Update(shift);

                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public IQueryable<RequestedShiftVM> RequestedShiftTable()
        {
            var data = from t1 in _db.Shifts
                       join t2 in _db.Shiftdetails on t1.Shiftid equals t2.Shiftid
                       where t2.Isdeleted != true && t2.Status !=2
                       select new RequestedShiftVM()
                       {
                           shiftDetailId = t2.Shiftdetailid,
                           staff = _db.Physicians.FirstOrDefault(x => x.Physicianid == t1.Physicianid).Firstname + " " +
                            _db.Physicians.FirstOrDefault(x => x.Physicianid == t1.Physicianid).Lastname,

                           Day = t2.Shiftdate.ToString(),
                           Time = t2.Starttime.ToString() + "-" + t2.Endtime.ToString(),
                           Region = _db.Regions.FirstOrDefault(x => x.Regionid == t2.Regionid).Name ?? "-",
                           RegionId = t2.Regionid ?? 0
                       };
            return data;
        }
    }
}
