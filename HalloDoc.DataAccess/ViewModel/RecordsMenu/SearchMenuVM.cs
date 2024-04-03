using HalloDoc.Services;
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
    }
}
