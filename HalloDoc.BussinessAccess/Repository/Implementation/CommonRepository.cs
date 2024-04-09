using HalloDoc.BussinessAccess.Repository.Interface;
using HalloDoc.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Reflection;
using OfficeOpenXml;
using HalloDoc.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;  
using System.Text.Json;


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

        public byte[] fileToExcel<T>(IEnumerable<T> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");

                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }
                int row = 2;

                foreach (var item in data)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
                    }
                    row++;
                }

                byte[] excelBytes = package.GetAsByteArray();

                return excelBytes;
            }

        }

    }
}
