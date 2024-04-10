using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IPhysicianSiteRepository
    {
        public int GetPhysicianId(string AspId);

        public countRequestViewModel DashboardCount(int phyId);
        public IQueryable<pendingReqViewModel> newReq(searchViewModel? obj, int phyId);
        public IQueryable<pendingReqViewModel> pendingReq(searchViewModel? obj, int phyId);

        public IQueryable<pendingReqViewModel> activeReq(searchViewModel? obj, int phyId);
        public IQueryable<pendingReqViewModel> concludeReq(searchViewModel? obj, int phyId);

        public MonthScheduling monthScheduling(string date, int phyId);
        public List<Region> CreateShiftRegion(int phyId);

        public IEnumerable<DayScheduling> ViewAllShift(string date, int phyId);

        public bool  AccpetRequest(int reqClientId);
    }
}
