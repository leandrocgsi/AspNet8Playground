using APIAspNetCore5.Business;
using APIAspNetCore5.Data.VO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace APIAspNetCore5.Controllers
{
    [ApiVersion("1")]
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class FileController : Controller
    {
        private readonly IFileBusiness _fileBusiness;
        public FileController(IFileBusiness fileBusiness)
        {
            _fileBusiness = fileBusiness;
        }

        [HttpPost("multi-upload")]
        [ProducesResponseType((200), Type = typeof(List<FileDetailVO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UploadManyFiles([FromForm] List<IFormFile> files)
        {
            List<FileDetailVO> list = await _fileBusiness.SaveFilesToDiskAsync(files);
            return new OkObjectResult(list);
        }

        [HttpPost("single-upload")]
        [ProducesResponseType((200), Type = typeof(FileDetailVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailVO detail = await _fileBusiness.SaveFileToDiskAsync(file);
            return new OkObjectResult(detail);
        }

        [HttpGet("{fileName}")]
        [ProducesResponseType((200), Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/pdf")]
        [Authorize("Bearer")]
        public IActionResult GetFile(string fileName)
        {
            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            FileStream file = _fileBusiness.GetFile(fileName);
            return File(file, "application/pdf");
        }
    }
}