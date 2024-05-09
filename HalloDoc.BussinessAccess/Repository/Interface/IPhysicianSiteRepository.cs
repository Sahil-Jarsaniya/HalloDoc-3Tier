using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.DataAccess.ViewModel.PhysicianDashboard;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IPhysicianSiteRepository
    {
        public int GetPhysicianId(string AspId);

        public List<Region> GetRegion();
        public List<Admin> GetAdminList();
        public bool PhysicianLocationUpdate(double latitude, double longitude, int phyId);
        public countRequestViewModel DashboardCount(int phyId);
        public IQueryable<pendingReqViewModel> newReq(searchViewModel? obj, int phyId);
        public IQueryable<pendingReqViewModel> pendingReq(searchViewModel? obj, int phyId);

        public IQueryable<pendingReqViewModel> activeReq(searchViewModel? obj, int phyId);
        public IQueryable<pendingReqViewModel> concludeReq(searchViewModel? obj, int phyId);

        public MonthScheduling monthScheduling(string date, int phyId);
        public List<Region> CreateShiftRegion(int phyId);

        public IEnumerable<DayScheduling> ViewAllShift(string date, int phyId);

        public bool AccpetRequest(int reqClientId);
        public bool Encounter(int reqClientId, string option);

        public bool HouseCallBtn(int id);

        public bool FinalizeEncounter(int id);

        public Encounter Encounter(int id);

        public bool TransferCase(int reqClientId, string note);

        public void ViewNotePost(int reqClientId, string Note, int phyId, string phyAspId);

        public void ConcludeCare(CloseCaseViewModel obj, string aspId);
        
        public void viewUplodPost(string file, int reqId,  int phyId);

        public IQueryable<BiWeeklyRecieptVM> ReceiptData(string date, int phyId);

        public bool isFinalizedSheet(string date, int phyId);

        public DateVM biweeklySheetVMs(string date, int phyId);

        public void biweeklySheetVMs(DateVM obj, int phyId);

        public void BiWeeklyReciept(BiWeeklyRecieptVM obj, int phyId);
    }
}
