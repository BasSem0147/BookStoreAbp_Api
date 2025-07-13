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
using Acme.BookStore.IServices.File;

namespace Acme.BookStore.AppServices.Files.Image
{
    public class FileServices : Acme.BookStore.IAppServices.IAppServices,IFileServices
    {
        private readonly IWebHostEnvironment _env;

        public FileServices(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpPost("UploadFileAsync")]
        public async Task<bool> UploadFileAsync(string Id, IFormFile file)
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
            return true;
        }
        [HttpPost("DeleteFileAsync")]
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
        [HttpGet("GetFileUrlAsync")]

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
        [ApiExplorerSettings(IgnoreApi = true)]
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
