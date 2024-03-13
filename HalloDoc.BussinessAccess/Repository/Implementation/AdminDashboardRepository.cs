using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class AdminDashboardRepository: IAdminDashboardRepository
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
                              reqTypeId = req.Requesttypeid,
                              Regionid = rc.Regionid,
                              Email = rc.Email,
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
                                 physicianName = phy.Firstname + " " + phy.Lastname,
                                 Regionid = rc.Regionid,
                                 Email = rc.Email,
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
                                 physicianName = phy.Firstname + " " + phy.Lastname,
                                 Regionid = rc.Regionid,
                                 Email = rc.Email,
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
                                 physicianName = phy.Firstname + " " + phy.Lastname,
                                 Regionid = rc.Regionid,
                                 Email = rc.Email,
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
                                 physicianName = phy.Firstname + " " + phy.Lastname,
                                 Regionid = rc.Regionid,
                                 Email = rc.Email,
                             };
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
                                    Notes = rc.Notes,
                                    reqTypeId = req.Requesttypeid,
                                    physicianName = phy.Firstname + " " + phy.Lastname,
                                    Regionid = rc.Regionid,
                                    Email = rc.Email,
                                };

            var CaseTag = from t1 in _db.Casetags select t1;
            var Region = from t1 in _db.Regions select t1;

            var data = new AdminDashboardViewModel
            {
                countRequestViewModel = count,
                newReqViewModel = newReqData,
                concludeReqViewModel = concludeReqData,
                closeReqViewModels = closeReqData,
                activeReqViewModels = activeReqData,
                pendingReqViewModel = pendingReqData,
                unpaidReqViewModels = unpaidReqData,
                Casetag = CaseTag,
                Region = Region

            };
            return data;
        }

        public AdminDashboardViewModel searchPatient(searchViewModel obj, AdminDashboardViewModel data)
        {
            if (obj.Name == null && obj.RegionId == 0 && obj.reqType ==0)
            {
                return data;
            }
            if (obj.Name != null)
            {
                var name = obj.Name.ToUpper();
                var sortedNew = data.newReqViewModel.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedConclude = data.concludeReqViewModel.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedClose = data.closeReqViewModels.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedPending = data.pendingReqViewModel.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedUnpaid = data.unpaidReqViewModels.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedActive = data.activeReqViewModels.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));

                data.newReqViewModel = sortedNew;
                data.concludeReqViewModel = sortedConclude;
                data.closeReqViewModels = sortedClose;
                data.pendingReqViewModel = sortedPending;
                data.activeReqViewModels = sortedActive;
                data.unpaidReqViewModels = sortedUnpaid;

            }

            if (obj.RegionId != 0)
            {
                var sortedNew = data.newReqViewModel.Where(s => s.Regionid == obj.RegionId);
                var sortedConclude = data.concludeReqViewModel.Where(s => s.Regionid == obj.RegionId);
                var sortedClose = data.closeReqViewModels.Where(s => s.Regionid == obj.RegionId);
                var sortedPending = data.pendingReqViewModel.Where(s => s.Regionid == obj.RegionId);
                var sortedUnpaid = data.unpaidReqViewModels.Where(s => s.Regionid == obj.RegionId);
                var sortedActive = data.activeReqViewModels.Where(s => s.Regionid == obj.RegionId);

                data.newReqViewModel = sortedNew;
                data.concludeReqViewModel = sortedConclude;
                data.closeReqViewModels = sortedClose;
                data.pendingReqViewModel = sortedPending;
                data.activeReqViewModels = sortedActive;
                data.unpaidReqViewModels = sortedUnpaid;

            }
            if(obj.reqType != 0)
            {
                var sortedNew = data.newReqViewModel.Where(s => s.reqTypeId == obj.reqType);
                var sortedConclude = data.concludeReqViewModel.Where(s => s.reqTypeId == obj.reqType);
                var sortedClose = data.closeReqViewModels.Where(s => s.reqTypeId == obj.reqType);
                var sortedPending = data.pendingReqViewModel.Where(s => s.reqTypeId == obj.reqType);
                var sortedUnpaid = data.unpaidReqViewModels.Where(s => s.reqTypeId == obj.reqType);
                var sortedActive = data.activeReqViewModels.Where(s => s.reqTypeId == obj.reqType);

                data.newReqViewModel = sortedNew;
                data.concludeReqViewModel = sortedConclude;
                data.closeReqViewModels = sortedClose;
                data.pendingReqViewModel = sortedPending;
                data.activeReqViewModels = sortedActive;
                data.unpaidReqViewModels = sortedUnpaid;
            }
            

                return data;
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
            var status= _db.Requests.Where(x => x.Requestid == ReqId.Requestid).FirstOrDefault().Status;    
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

            var reqBlock = new Blockrequest
            {
                Phonenumber = reqId.Phonenumber,
                Email = reqId.Email,
                Reason = addNote,
                Requestid = _db.Users.Where(x => x.Userid == reqCol.Userid).FirstOrDefault().Aspnetuserid,
                Createddate = DateTime.Now
            };
            _db.Blockrequests.Add(reqBlock);
            _db.SaveChanges();
        }

        public object FilterPhysician(int Region)
        {
            var physicians = (from t1 in _db.Physicianregions
                              join t2 in _db.Physicians on t1.Physicianid equals t2.Physicianid
                              where t1.Regionid == Region
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
            reqRow.Status = 2;
            reqRow.Physicianid = PhysicianSelect;
            reqRow.Modifieddate = DateTime.Now;
            _db.Requests.Update(reqRow);
            _db.SaveChanges();

            var reqNote = _db.Requestnotes.Where(x => x.Requestid == reqRow.Requestid).FirstOrDefault();

            if(reqNote == null)
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
                PatientName = reqId.Firstname+ " "+reqId.Lastname,
                ConfirmationNo = reqRow.Confirmationnumber,
                PatientDocumentViewModel = requestData,
                UploadFileViewModel = uploadData
            };

            return data;
        }
    
        public void DeleteFile(int reqClientId, string FileName){
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
    }
}