using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface ICommonRepository
    {
        public void uploadFile(string fileName, int reqId);
    }
}
