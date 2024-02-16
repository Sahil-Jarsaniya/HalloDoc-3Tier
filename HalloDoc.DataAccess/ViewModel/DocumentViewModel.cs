using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DataAccess.ViewModel
{
    public class DocumentViewModel
    {

        public UploadFileViewModel UploadFileViewModel { get; set; }

        public IEnumerable<PatientDocumentViewModel> PatientDocumentViewModel { get; set; }
    }
}
