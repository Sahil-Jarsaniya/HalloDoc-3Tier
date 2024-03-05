using Microsoft.AspNetCore.Http;

namespace HalloDoc.DataAccess.ViewModel
{
    public class UploadFileViewModel
    {
        public int reqId { get; set; }

        public IFormFile? formFile { get; set; }
    }
}
