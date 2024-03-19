using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface ISMSSender
    {
        public Task<bool> SendSmsAsync(string to, string message);
    }
}
