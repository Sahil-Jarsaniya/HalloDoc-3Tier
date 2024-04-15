using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel.AdminViewModel
{
    public class viewNoteViewModel
    {
        public int status { get; set; }
        public int reqClientId { get; set; }

        [Required]
        public String adminNote { get; set; }
        public IEnumerable<Requestnote> Requestnote { get; set; }
        public IEnumerable<Requeststatuslog> Requeststatuslog { get; set; }
    }
}
