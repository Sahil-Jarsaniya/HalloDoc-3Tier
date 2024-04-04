using HalloDoc.DataAccess.Models;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.RecordsMenu
{
    public class SearchMenuVM
    {
        public SearchSortingVM? search { get; set; }

        public PaginatedList<SearchTableVM>? searchTableVMs { get; set; }

        public PaginatedList<BlockHistoryVM>? BlockHistoryVM { get; set; }

        public IEnumerable<Role>? roles { get; set; }
    }
}
