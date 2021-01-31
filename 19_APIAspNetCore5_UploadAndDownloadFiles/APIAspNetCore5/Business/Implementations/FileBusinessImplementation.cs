using APIAspNetCore5.Data.VO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace APIAspNetCore5.Business.Implementations
{
    public class FileBusinessImplementation : IFileBusiness
    {
        private readonly String path;

        public FileBusinessImplementation()
        {
            path = Directory.GetCurrentDirectory();
        }
        public byte[] GetPDFFile()
        {
            
            var fullPath = path + "\\Other\\aspnet-life-cycles-events.pdf";
            return File.ReadAllBytes(fullPath);
        }

        public FileDetailVO SaveFileToDisk(IFormFile file)
        {
            FileDetailVO fileDetail = new FileDetailVO();
            var fileType = Path.GetExtension(file.FileName);

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var fullPath = path + "\\Other\\UploadDir\\";
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    fileDetail.DocumentName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocUrl = Path.Combine(fullPath, "", fileDetail.DocumentName);
                    using (var stream = new FileStream(fileDetail.DocUrl, FileMode.Create))
                    {
                        _ = file.CopyToAsync(stream);
                    }
                }
            }

            return fileDetail;
        }

        public List<FileDetailVO> SaveFilesToDisk(IList<IFormFile> files)
        {
            List<FileDetailVO> list = new List<FileDetailVO>();
            foreach (var file in files)
            {
                list.Add(SaveFileToDisk(file));
            }
            return list;
        }
    }
}
