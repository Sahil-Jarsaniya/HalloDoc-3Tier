    using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.ViewModel.RecordsMenu;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;

namespace HalloDoc.Controllers
{
    [CustomAuth("Admin")]
    public class RecordsMenuController : Controller
    {
        private readonly IRecordsRepository _recordsRepo;
        private readonly INotyfService _noty;
        private readonly IJwtService _jwtService;
        public RecordsMenuController(IRecordsRepository recordsRepo, INotyfService noty, IJwtService jwtService)
        {
            _recordsRepo = recordsRepo;
            _noty = noty;
            _jwtService = jwtService;

        }
        public string GetAdminName()
        {
            var token = Request.Cookies["jwt"];
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string fname = jwt.Claims.First(c => c.Type == "firstName").Value;
            string lname = jwt.Claims.First(c => c.Type == "lastName").Value;

            return fname + "_" + lname;
        }
        public IActionResult SearchRecords()
        {
            ViewBag.AdminName = GetAdminName();

            return View();
        }
        public async Task<IActionResult> SearchRecordsTable(int pagenumber, SearchSortingVM obj)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;

            var data = _recordsRepo.SearchRecords();

            if (obj != null)
            {
                if (obj.Phonenumber != null)
                {
                    data = data.Where(x => x.PhoneNumber == obj.Phonenumber);
                }
                if (obj.ProviderName != null)
                {
                    data = data.Where(x => x.Physician.ToUpper().Contains(obj.ProviderName.ToUpper()));
                }
                if (obj.PatientName != null)
                {
                    data = data.Where(x => x.PatientName.ToUpper().Contains(obj.PatientName.ToUpper()));
                }
                if (obj.Email != null)
                {
                    data = data.Where(x => x.Email == obj.Email);
                }
                if (obj.ReqStatus != null && obj.ReqStatus != 0)
                {
                    data = data.Where(x => x.ReqStatusId == obj.ReqStatus);
                }
                if (obj.ReqType != null && obj.ReqType != 0)
                {
                    data = data.Where(x => x.ReqTypeId == obj.ReqType);
                }
                if (obj.Date1.Month != 1 && obj.Date1.Year != 1 && obj.Date1.Day != 1)
                {
                    data = data.Where(x => x.DateOfService.Year == obj.Date1.Year &&
                    x.DateOfService.Month == obj.Date1.Month &&
                    x.DateOfService.Day == obj.Date1.Day
                    );
                }
                if (obj.Date2.Month != 1 && obj.Date2.Year != 1 && obj.Date2.Day != 1)
                {
                    data = data.Where(x => x.CloseCaseDate.Year == obj.Date2.Year &&
                    x.CloseCaseDate.Month == obj.Date2.Month &&
                    x.CloseCaseDate.Day == obj.Date2.Day
                    );
                }
            }

            return PartialView("_SearchRecordsTable", await PaginatedList<SearchTableVM>.CreateAsync(data, pagenumber, pageSize));
        }

        public IActionResult DeleteRecord(int id)
        {
            bool x = _recordsRepo.DeleteRecords(id);
            if (x)
            {
                _noty.Success("Records Deleted");
            }
            else
            {
                _noty.Error("Something went wrong.");
            }
            return RedirectToAction("SearchRecords");
        }

        public IActionResult BlockHistory()
        {
            ViewBag.AdminName = GetAdminName();
            return View();
        }
        public async Task<IActionResult> BlockHistoryTable(int pagenumber, SearchSortingVM obj)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;

            var data = _recordsRepo.BlockHistory();

            if (obj != null)
            {
                if (obj.Phonenumber != null)
                {
                    data = data.Where(x => x.PhoneNumber == obj.Phonenumber);
                }
                if (obj.PatientName != null)
                {
                    data = data.Where(x => x.PatientName.ToUpper().Contains(obj.PatientName.ToUpper()));
                }
                if (obj.Email != null)
                {
                    data = data.Where(x => x.Email == obj.Email);
                }
                if (obj.Date1.Month != 1 && obj.Date1.Year != 1 && obj.Date1.Day != 1)
                {
                    data = data.Where(x => x.CreatedDate.Year == obj.Date1.Year &&
                    x.CreatedDate.Month == obj.Date1.Month &&
                    x.CreatedDate.Day == obj.Date1.Day
                    );
                }
            }
            return PartialView("_BlockHistoryTable", await PaginatedList<BlockHistoryVM>.CreateAsync(data, pagenumber, pageSize));
        }

        public IActionResult UnBlockPatient(int id)
        {
            bool x = _recordsRepo.UnBlock(id);

            if (x)
            {
                _noty.Success("User Unblocked");
            }
            else
            {
                _noty.Error("Something went wrong.");
            }

            return RedirectToAction("BlockHistory");
        }

        public IActionResult EmailLogs()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new SearchMenuVM
            {
                roles = _recordsRepo.roles()
            };
            return View(data);
        }

        public IActionResult SmsLogs()
        {
            ViewBag.AdminName = GetAdminName();
            var data = new SearchMenuVM
            {
                roles = _recordsRepo.roles()
            };
            return View(data);
        }

        public async Task<IActionResult> EmailLogTable(int pagenumber, SearchSortingVM obj)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;

            var data = _recordsRepo.EmailLogs();

            if (obj != null)
            {
                if (obj.Email != null)
                {
                    data = data.Where(x => x.EmailId == obj.Email);
                }
                if (obj.PatientName != null)
                {
                    data = data.Where(x => x.Recipient.ToUpper().Contains(obj.PatientName.ToUpper()));
                }
                if (obj.RoleId != 0 && obj.RoleId != null)
                {
                    data = data.Where(x => x.RoleId == obj.RoleId);
                }
                if (obj.Date1.Month != 1 && obj.Date1.Year != 1 && obj.Date1.Day != 1)
                {
                    data = data.Where(x => x.CreateDate.Year == obj.Date1.Year &&
                    x.CreateDate.Month == obj.Date1.Month &&
                    x.CreateDate.Day == obj.Date1.Day
                    );
                }
                if (obj.Date2.Month != 1 && obj.Date2.Year != 1 && obj.Date2.Day != 1)
                {
                    data = data.Where(x => x.SentDate.Year == obj.Date2.Year &&
                    x.SentDate.Month == obj.Date2.Month &&
                    x.SentDate.Day == obj.Date2.Day &&
                    x.SentDate.Month != 1 && x.SentDate.Year != 1 && x.SentDate.Day != 1
                    );
                }
            }
            return PartialView("_EmailLogTable", await PaginatedList<EmailLogVM>.CreateAsync(data, pagenumber, pageSize));
        }

        public async Task<IActionResult> SmsLogTable(int pagenumber, SearchSortingVM obj)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;

            var data = _recordsRepo.SmsLogs();

            if (obj != null)
            {
                if (obj.Phonenumber != null)
                {
                    data = data.Where(x => x.PhoneNumber == obj.Phonenumber);
                }
                if (obj.PatientName != null)
                {
                    data = data.Where(x => x.Recipient.ToUpper().Contains(obj.PatientName.ToUpper()));
                }
                if (obj.RoleId != 0 && obj.RoleId != null)
                {
                    data = data.Where(x => x.RoleId == obj.RoleId);
                }
                if (obj.Date1.Month != 1 && obj.Date1.Year != 1 && obj.Date1.Day != 1)
                {
                    data = data.Where(x => x.CreateDate.Year == obj.Date1.Year &&
                    x.CreateDate.Month == obj.Date1.Month &&
                    x.CreateDate.Day == obj.Date1.Day
                    );
                }
                if (obj.Date2.Month != 1 && obj.Date2.Year != 1 && obj.Date2.Day != 1)
                {
                    data = data.Where(x => x.SentDate.Year == obj.Date2.Year &&
                    x.SentDate.Month == obj.Date2.Month &&
                    x.SentDate.Day == obj.Date2.Day &&
                    x.SentDate.Month != 1 && x.SentDate.Year != 1 && x.SentDate.Day != 1
                    );
                }
            }
            return PartialView("_SmsLogTable", await PaginatedList<EmailLogVM>.CreateAsync(data, pagenumber, pageSize));
        }

        public IActionResult PatientHistory()
        {
            return View();
        }

        public async Task<IActionResult> PatientHistoryTable(int pagenumber, SearchSortingVM obj)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;

            var data = _recordsRepo.PatientHistory();

            if (obj != null)
            {
                if (obj.Phonenumber != null)
                {
                    data = data.Where(x => x.phoneNumber == obj.Phonenumber);
                }
                if (obj.PatientName != null)
                {
                    data = data.Where(x => x.firstName.ToUpper().Contains(obj.PatientName.ToUpper()));
                }
                if (obj.ProviderName != null)
                {
                    data = data.Where(x => x.lastName.ToUpper().Contains(obj.ProviderName.ToUpper()));
                }
                if (obj.Email != null)
                {
                    data = data.Where(x => x.email == obj.Email);
                }
            }
            return PartialView("_PatientHistoryTable", await PaginatedList<PatientHistoryVM>.CreateAsync(data, pagenumber, pageSize));
        }

        public async Task<IActionResult> PatientRecord(int reqId, int pagenumber)
        {
            if (pagenumber < 1)
            {
                pagenumber = 1;
            }
            var pageSize = 2;

            var data  = _recordsRepo.PatientRecord(reqId);

            return View(await PaginatedList<PatientRecordVM>.CreateAsync(data, pagenumber, pageSize));
        }
    }
}
