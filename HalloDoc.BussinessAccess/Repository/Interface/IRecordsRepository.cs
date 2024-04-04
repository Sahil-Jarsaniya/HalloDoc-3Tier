using HalloDoc.DataAccess.Models;
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

        public IEnumerable<Role> roles();
        
        public IQueryable<EmailLogVM> EmailLogs();
        public IQueryable<EmailLogVM> SmsLogs();

        public IQueryable<PatientHistoryVM> PatientHistory();
        public IQueryable<PatientRecordVM> PatientRecord(int reqId);
    }
}
