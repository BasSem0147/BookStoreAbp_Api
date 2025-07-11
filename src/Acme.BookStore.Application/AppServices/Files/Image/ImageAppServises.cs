using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp;
using Acme.BookStore.IAppServices;

namespace Acme.BookStore.AppServices.Files.Image
{
    public class ImageAppServises : Acme.BookStore.IAppServices.IAppServices, IFileServices
    {
        private readonly IWebHostEnvironment _env;

        public ImageAppServises(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("api/app/image/upload")]
        public async Task<string> UploadFileAsync(string Id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new UserFriendlyException("No file uploaded.");
            }
            var filePath = Path.Combine("wwwroot", GetUploadDirectory(Id) + Guid.NewGuid().ToString() + "_" + file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"uploads/{filePath}";
        }

        public async Task<bool> DeleteFileAsync(string Id)
        {
            string path = GetUploadDirectory(Id);
            if (Directory.Exists(path))
            {
                await Task.Run(() => { Directory.Delete(path, true); });
                return true;
            }
            return false;
        }
        public async Task<string> GetFileUrlAsync(string Id)
        {
            var path = Path.Combine("wwwroot", GetUploadDirectory(Id));
            if (Directory.Exists(path))
            {
                var fileUrl = Directory.GetFiles(path).Select(file =>
                {
                    var relativePath = Path.GetRelativePath("wwwroot", file);
                    return "/" + relativePath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                }).FirstOrDefault();

                return fileUrl;
            }
            return "";
        }
        internal string GetUploadDirectory(string Id)
        {
            var dir = Path.GetFullPath(".\\wwwroot");
            string dirPath = dir + @$"\\Uploads\\{Id.ToString()}\\";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
    }
}
