using APIAspNetCore5.Data.VO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace APIAspNetCore5.Business
{
    public interface IFileBusiness
    {
        public FileStream GetFile(string fileName);
        public Task<FileDetailVO> SaveFileToDiskAsync(IFormFile file);
        public Task<List<FileDetailVO>> SaveFilesToDiskAsync(IList<IFormFile> files);
    }
}