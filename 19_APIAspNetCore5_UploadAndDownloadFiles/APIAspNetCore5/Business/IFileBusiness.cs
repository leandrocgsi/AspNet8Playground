using APIAspNetCore5.Data.VO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace APIAspNetCore5.Business
{
    public interface IFileBusiness
    {
        byte[] GetPDFFile();
        public FileDetailVO SaveFileToDisk(IFormFile file);
        public List<FileDetailVO> SaveFilesToDisk(IList<IFormFile> files);
    }
}