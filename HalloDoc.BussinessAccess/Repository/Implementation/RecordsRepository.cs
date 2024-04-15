using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.RecordsMenu;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var data = from t1 in _db.Blockrequests
                       select new BlockHistoryVM()
                       {
                           PatientName = _db.Requestclients.FirstOrDefault(x => x.Requestid == t1.Requestid).Firstname
                           + " " + _db.Requestclients.FirstOrDefault(x => x.Requestid == t1.Requestid).Lastname,
                           Email = t1.Email,
                           CreatedDate = (DateTime)t1.Createddate,
                           Notes = t1.Reason,
                           ReqId = t1.Requestid,
                           isActive = t1.Isactive,
                           PhoneNumber = _db.Requestclients.FirstOrDefault(x => x.Requestid == t1.Requestid).Phonenumber
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

               
                var block = _db.Blockrequests.FirstOrDefault(x => x.Requestid == reqId);
                block.Isactive = true;
                _db.Blockrequests.Update(block);    
                _db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public IEnumerable<Role> roles()
        {
             return _db.Roles; 
        }

        public IQueryable<EmailLogVM> EmailLogs()
        {
            var data = from t1 in _db.Emaillogs
                       join t2 in _db.Roles on t1.Roleid equals t2.Roleid
                       select new EmailLogVM()
                       {
                           id = t1.Emaillogid,
                           EmailId = t1.Emailid,
                           ConfirmationNumber = t1.Confirmationnumber == null? "-" : t1.Confirmationnumber,
                           CreateDate = t1.Createdate,
                           SentDate = (DateTime)(t1.Sentdate == null? DateTime.MinValue : t1.Sentdate),
                           sentTries = t1.Senttries,
                           sent = t1.Isemailsent,
                           RoleId = t1.Roleid,
                           Rolename = t2.Name,
                       };

            return data;

        }
        public IQueryable<EmailLogVM> SmsLogs()
        {
            var data = from t1 in _db.Smslogs
                       join t2 in _db.Roles on t1.Roleid equals t2.Roleid
                       select new EmailLogVM()
                       {
                           PhoneNumber = t1.Mobilenumber,
                           ConfirmationNumber = t1.Confirmationnumber == null ? "-" : t1.Confirmationnumber,
                           CreateDate = t1.Createdate,
                           SentDate = (DateTime)(t1.Sentdate == null ? DateTime.MinValue : t1.Sentdate),
                           sentTries = t1.Senttries,
                           sent = t1.Issmssent,
                           RoleId = t1.Roleid,
                           Rolename = t2.Name,
                           id = (int)t1.Smslogid
                       };

            return data;
        }

        public IQueryable<PatientHistoryVM> PatientHistory()
        {
            var data = from t1 in _db.Users
                       select new PatientHistoryVM()
                       {
                           firstName = t1.Firstname,
                           lastName = t1.Lastname,
                           Address = t1.City + ", " + t1.Street+", "+ t1.State,
                           email = t1.Email,
                           phoneNumber = t1.Mobile,
                           reqId = t1.Userid,
                       };
            return data;    
        }

        public IQueryable<PatientRecordVM> PatientRecord(int reqId)
        {
            var data = from t0 in _db.Users
                       join t1 in _db.Requests on t0.Userid equals t1.Userid
                       join t2 in _db.Requestclients on t1.Requestid equals t2.Requestid
                       where t0.Userid == reqId
                       select new PatientRecordVM()
                       {
                           Client = t0.Firstname + " " + t0.Lastname,
                           createDate = t1.Createddate,
                           confirmation = t1.Confirmationnumber,
                           providerName = _db.Physicians.FirstOrDefault(x => x.Physicianid ==  t1.Physicianid).Firstname ?? "-",
                           status = _db.RequestStatuses.FirstOrDefault(x => x.StatusId == t1.Status).Status,
                           reqClientId = t2.Requestclientid,
                           reqId = t1.Requestid
                       };
            return data;
        }
    }
}
