using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VoxelMapsTestTask.Enums;
using VoxelMapsTestTask.Hub;
using VoxelMapsTestTask.Models;

namespace VoxelMapsTestTask.Factory
{
    public static class FileHandleFactory
    {
        public async static void StartOperation(FileHandleRequestModel fileHandleRequest, CancellationToken cancellationToken, IHubContext<ProgressHub> _hubContext)
        {
            int filesHandledCount = 0;
            int filesCount = fileHandleRequest.FilesToHandle.Count;

            var progress = new FileHandleProgressModel();

            foreach (var file in fileHandleRequest.FilesToHandle)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    progress.Message = "Operation canceled!";
                    progress.IsCanceled = true;
                    await _hubContext.Clients.Client(fileHandleRequest.HubConnectionId).SendAsync("ProgressUpdate", progress);
                    break;
                }

                var fileName = Path.GetFileName(file);
                var dist = Path.Combine(fileHandleRequest.DestinationFolder, fileName);

                if (fileHandleRequest.Type.Equals(FileHandleTypeEnum.Copy)) File.Copy(file, dist, false);
                else File.Move(file, dist, false);

                //Calculating the progress percentage
                filesHandledCount = filesHandledCount + 1;
                int progressPercentage = (filesHandledCount * 100) / filesCount;
                progress.Percentage = progressPercentage;
                
                if (progressPercentage == 100)
                    progress.Message = "Operation completed successfully!";
                else
                    progress.Message = $"The {fileName} file has been copied.";

                await _hubContext.Clients.Client(fileHandleRequest.HubConnectionId).SendAsync("ProgressUpdate", progress);

                //Just for test cancelation task method and could see the progress in the frontend
                //Without that the operation is too much fast
                Task.Delay(1000).Wait();
            }
        }
    }
}
