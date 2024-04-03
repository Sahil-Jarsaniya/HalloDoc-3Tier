using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.ViewModel.RecordsMenu;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HalloDoc.Controllers
{
    public class RecordsMenuController : Controller
    {
        private readonly IRecordsRepository _recordsRepo;
        private readonly INotyfService _noty;
        public RecordsMenuController(IRecordsRepository recordsRepo, INotyfService noty)
        {
            _recordsRepo = recordsRepo;
            _noty = noty;
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
                if(obj.Date1.Month != 1 && obj.Date1.Year != 1 && obj.Date1.Day != 1) 
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
    }
}
