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
        private readonly string path;
        private readonly IHttpContextAccessor _context;
        public FileBusinessImplementation(IHttpContextAccessor context)
        {
            _context = context;
            path = Directory.GetCurrentDirectory();
        }
        public FileStream GetFile(string fileName)
        {

            var fullPath = path + $"\\Other\\UploadDir\\{fileName}";
            var file = File.OpenRead(fullPath);
            return file;
        }

        public async Task<FileDetailVO> SaveFileToDiskAsync(IFormFile file)
        {
            FileDetailVO fileDetail = new FileDetailVO();
            var fileType = Path.GetExtension(file.FileName);
            var baseURL = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var fullPath = path + "\\Other\\UploadDir\\";
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(fullPath, "", docName);
                    fileDetail.DocumentName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocUrl = Path.Combine(baseURL + "/api/file/v1/", fileDetail.DocumentName);
                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<FileDetailVO>> SaveFilesToDiskAsync(IList<IFormFile> files)
        {
            List<FileDetailVO> list = new List<FileDetailVO>();
            foreach (var file in files)
            {
                list.Add(await SaveFileToDiskAsync(file));
            }
            return list;
        }

    }
}
