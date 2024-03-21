using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using HalloDoc.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Implementation
{
    public class CommonRepository: ICommonRepository
    {
        private readonly ApplicationDbContext _db;

        public CommonRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void uploadFile(IFormFile? file, string path)
        {
            //uploading files
            if (file != null && file.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(file.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploadedFiles", path);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }

    }
}
