using Microsoft.AspNetCore.Http;
using RestWithASPNETErudio.Data.VO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestWithASPNETErudio.Business
{
    public interface IFileBusiness
    {
        public byte[] GetFile(string filename);
        public Task<FileDetailVO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> file);
    }
}
