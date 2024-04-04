using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel.ProvidersMenu;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IProviderMenuRepository
    {
        public IEnumerable<Region> Regions();

        public void CreateShift(string selectedDays, CreateShift obj, string AspId);

        public IEnumerable<DayScheduling> DayWiseScheduling(string date);
        public WeekScheduling WeekWiseScheduling(string date);

        public CreateShift ViewShift(int shiftDetailId);

        public bool DeleteShift(int shiftDetailId); 
        public bool UpdateShift(CreateShift obj, int id);

        public IQueryable<RequestedShiftVM> RequestedShiftTable();
    }
}
