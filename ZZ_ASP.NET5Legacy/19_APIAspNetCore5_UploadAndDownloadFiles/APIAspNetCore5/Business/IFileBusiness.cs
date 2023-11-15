using APIAspNetCore5.Data.VO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace APIAspNetCore5.Business
{
    public interface IFileBusiness
    {
        public byte[] GetFile(string fileName);
        public Task<FileDetailVO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> files);
    }
}