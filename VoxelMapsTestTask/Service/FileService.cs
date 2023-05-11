using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoxelMapsTestTask.Factory;
using VoxelMapsTestTask.Hub;
using VoxelMapsTestTask.Models;
using Microsoft.AspNetCore.SignalR;

namespace VoxelMapsTestTask.Service
{
    public class FileService : IFileService
    {
        private readonly IHubContext<ProgressHub> _hubContext;



        public FileService(IHubContext<ProgressHub> hubContext) => _hubContext = hubContext;


        /// <summary>
        /// Return the list of all files in source folders and  all available destination directories
        /// </summary>
        /// <returns>FileDirectoryInfoModel</returns>
        public async Task<FileDirectoryInfoModel> GetFiles()
        {
            var testDirectoriesPath = @$"{Environment.CurrentDirectory}\TestDirectories\";
            var files = Directory.GetFiles(testDirectoriesPath, "*", SearchOption.AllDirectories);

            var sourceFiles = files.Select((file) =>
            {
                var obj = new FileInfoModel
                {
                    FileName = Path.GetFileName(file),
                    FilePath = Path.GetDirectoryName(file)
                };

                return obj;
            });

            var testDestinationDirectoriesPath = @$"{Environment.CurrentDirectory}\TestDirectories\DestinationDirectories";
            var dirs = Directory.GetDirectories(testDestinationDirectoriesPath, "*", SearchOption.AllDirectories);

            var result = new FileDirectoryInfoModel
            {
                DestinationDirectories = dirs,
                SourceFiles = sourceFiles
            };

            return result;
        }

        /// <summary>
        /// Copy or move file(s) from the source diretory to the destination directory
        /// </summary>
        /// <param name="fileHandleRequest">File Handle request info</param>
        /// <returns></returns>
        public async Task HandleFiles(FileHandleRequestModel fileHandleRequest)
        {
            var _cancellationTokenSource = new CancellationTokenSource();
            TokenManager.RegisterCancellationToken(fileHandleRequest.HubConnectionId, _cancellationTokenSource);
            
            await Task.Run(() => FileHandleFactory.StartOperation(fileHandleRequest, _cancellationTokenSource.Token, _hubContext));
        }

        /// <summary>
        /// Cancel the requested operation (copy or move files)
        /// </summary>
        /// <param name="hubConnectionId"></param>
        /// <returns></returns>
        public async Task CancelHandleFiles(string hubConnectionId)
        {
            CancellationTokenSource _cancellationTokenSource = TokenManager.GetCancellationTokenSource(hubConnectionId);
            _cancellationTokenSource.Cancel();
        }
    }
}
