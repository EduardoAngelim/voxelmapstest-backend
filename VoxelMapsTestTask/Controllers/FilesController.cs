using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoxelMapsTestTask.Models;
using VoxelMapsTestTask.RequestHandle;
using VoxelMapsTestTask.Service;
using VoxelMapsTestTask.Utils;

namespace VoxelMapsTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;



        public FilesController(IFileService fileService) => _fileService = fileService;



        [HttpGet("list-files-directories")]
        public async Task<ActionResult<FileDirectoryInfoModel>> GetFiles()
        {
            try
            {
                var request = await _fileService.GetFiles();
                var result = new RequestResult<FileDirectoryInfoModel>(request, false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new RequestResult<FileDirectoryInfoModel>(null, true, RequestAnswer.FileListError.GetDescription(), ex.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("handle-files")]
        public async Task<ActionResult<RequestResult<RequestAnswer>>> HandleFiles([FromBody] FileHandleRequestModel fileHandleRequest)
        {
            try
            {
                await _fileService.HandleFiles(fileHandleRequest);
                var result = new RequestResult<IEnumerable<FileDirectoryInfoModel>>(null, false, RequestAnswer.FileHandleSuccess.GetDescription());
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new RequestResult<RequestAnswer>(RequestAnswer.FileHandleError, true, null, ex.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("cancel-handle-files/{hubConnectionId}")]
        public async Task<ActionResult<RequestResult<RequestAnswer>>> CancelHandleFiles([FromRoute] string hubConnectionId)
        {
            try
            {
                await _fileService.CancelHandleFiles(hubConnectionId);
                var result = new RequestResult<RequestAnswer>(RequestAnswer.FileHandleCanceledSuccess, false);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = new RequestResult<RequestAnswer>(RequestAnswer.FileHandleCanceledError, true, null, ex.Message);
                return BadRequest(result);
            }
        }
    }
}