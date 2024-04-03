using HalloDoc.DataAccess.ViewModel.RecordsMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IRecordsRepository
    {
        public IQueryable<SearchTableVM> SearchRecords();

        public bool DeleteRecords(int requestId);

        public IQueryable<BlockHistoryVM> BlockHistory();

        public bool UnBlock(int reqId);
    }
}
