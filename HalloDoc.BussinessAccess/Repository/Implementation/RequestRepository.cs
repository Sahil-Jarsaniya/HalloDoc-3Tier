using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class RequestRepository : IRequestRepository
    {
        public Task CreatePatientRequest(PatientViewModel obj)
        {

            return Task.CompletedTask;
        }
    }
}
