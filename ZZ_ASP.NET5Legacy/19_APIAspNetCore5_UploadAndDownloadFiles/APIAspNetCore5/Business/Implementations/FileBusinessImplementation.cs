using APIAspNetCore5.Data.VO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace APIAspNetCore5.Business.Implementations
{
    public class FileBusinessImplementation : IFileBusiness
    {
        private readonly string basePath;
        private readonly IHttpContextAccessor _context;
        public FileBusinessImplementation(IHttpContextAccessor context)
        {
            _context = context;
            basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }
        public byte[] GetFile(string fileName)
        {
            var filePath = basePath + fileName;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
        {
            FileDetailVO fileDetail = new FileDetailVO();
            var fileType = Path.GetExtension(file.FileName);
            var baseURL = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(basePath, "", docName);
                    fileDetail.DocumentName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocUrl = Path.Combine(baseURL + "/api/file/v1/", fileDetail.DocumentName);
                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> files)
        {
            List<FileDetailVO> list = new List<FileDetailVO>();
            foreach (var file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }
            return list;
        }

    }
}
