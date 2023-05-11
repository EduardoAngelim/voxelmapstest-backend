using System.Threading.Tasks;
using VoxelMapsTestTask.Models;

namespace VoxelMapsTestTask.Service
{
    public interface IFileService
    {
        Task<FileDirectoryInfoModel> GetFiles();
        Task HandleFiles(FileHandleRequestModel fileHandleRequest);
        Task CancelHandleFiles(string hubConnectionId);
    }
}
