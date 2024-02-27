using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class CommonRepository
    {
        private readonly ApplicationDbContext _db;

        public CommonRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void uploadFile(string file, int reqId)
        {
            ////uploading files
            //if (file!= null && file.Length > 0)
            //{
            //    //get file name
            //    var fileName = Path.GetFileName(file);

            //    //define path
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "././HalloDoc", "wwwroot", "uploadedFiles", fileName);

            //    // Copy the file to the desired location
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        file.CopyTo(stream);
            //    }
            //    Requestwisefile requestwisefile = new Requestwisefile
            //    {
            //        Filename = fileName,
            //        Requestid = request.Requestid,
            //        Createddate = DateTime.Now
            //    };

            //    _db.Requestwisefiles.Add(requestwisefile);
            //    _db.SaveChanges();
            //}
        }
    }
}
