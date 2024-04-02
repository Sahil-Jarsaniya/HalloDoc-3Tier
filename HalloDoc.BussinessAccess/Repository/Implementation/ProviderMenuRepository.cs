using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class ProviderMenuRepository: IProviderMenuRepository
    {
        private readonly ApplicationDbContext _db;

        public ProviderMenuRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Region> Regions()
        {
            return _db.Regions;
        }
    }
}
