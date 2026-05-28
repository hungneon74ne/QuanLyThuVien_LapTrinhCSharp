using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QuanLyThuVien.Helpers
{
    public static class FileUploadHelper
    {
        public static async Task<string> UploadImageAsync(IFormFile file, string webRootPath, string folderName = "images/sach")
        {
            if (file == null || file.Length == 0) return null;

            string uploadPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/{folderName}/{fileName}";
        }
    }
}
