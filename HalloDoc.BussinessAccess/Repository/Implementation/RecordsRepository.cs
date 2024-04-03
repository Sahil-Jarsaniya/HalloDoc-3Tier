using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.ViewModel.RecordsMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class RecordsRepository : IRecordsRepository
    {
        private readonly ApplicationDbContext _db;
        public RecordsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IQueryable<SearchTableVM> SearchRecords()
        {

            var tableData = (from t1 in _db.Requests
                             join t2 in _db.Requestclients on t1.Requestid equals t2.Requestid
                             join t3 in _db.RequestStatuses on t1.Status equals t3.StatusId
                             join t4 in _db.Physicians on t1.Physicianid equals t4.Physicianid into phy
                             from T4 in phy.DefaultIfEmpty()
                             join t5 in _db.Requestnotes on t1.Requestid equals t5.Requestid into note
                             from T5 in note.DefaultIfEmpty()
                             where t1.Isdeleted != true
                             select new SearchTableVM()
                             {
                                 PatientName = t2.Firstname + " " + t2.Lastname,
                                 Requestor = t1.Firstname + " " + t1.Lastname,
                                 DateOfService = t1.Createddate,
                                 Email = t2.Email,
                                 PhoneNumber = t2.Phonenumber,
                                 Address = t2.Address,
                                 Zip = t2.Zipcode,
                                 RequestStatus = t3.Status,
                                 Physician = T4.Firstname + " " + T4.Lastname,
                                 PhysicianNote = T5.Physiciannotes,
                                 AdminNote = T5.Adminnotes,
                                 CancelledProviderNote = t1.Casetag,
                                 PatientNote = t2.Notes,
                                 requestId = t1.Requestid,
                                 ReqStatusId = t3.StatusId,
                                 ReqTypeId = t1.Requesttypeid
                             });
            return tableData;
        }
        public bool DeleteRecords(int requestId)
        {
            try
            {
                var reqRow = _db.Requests.FirstOrDefault(x => x.Requestid == requestId);
                reqRow.Isdeleted = true;
                if (reqRow != null)
                {
                    _db.Requests.Update(reqRow);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<BlockHistoryVM> BlockHistory()
        {
            var data = from t1 in _db.Requestclients
                       join t2 in _db.Requests on t1.Requestid equals t2.Requestid
                       where t2.Status == 14
                       select new BlockHistoryVM()
                       {
                           PatientName = t1.Firstname + " " + t1.Lastname,
                           PhoneNumber = t1.Phonenumber,
                           Email = t1.Email,
                           CreatedDate = t2.Createddate,
                           Notes = t1.Notes,
                           ReqId = t2.Requestid,
                       };
            return data;
        }

        public bool UnBlock(int reqId)
        {
            try
            {

                var reqrow = _db.Requests.FirstOrDefault(x => x.Requestid == reqId);
                reqrow.Status = 1;
                _db.Requests.Update(reqrow);
                _db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}
