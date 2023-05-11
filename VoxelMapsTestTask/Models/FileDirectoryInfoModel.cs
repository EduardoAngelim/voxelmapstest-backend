using System.Collections.Generic;

namespace VoxelMapsTestTask.Models
{
    public class FileDirectoryInfoModel
    {
        public IEnumerable<string> DestinationDirectories { get; set; }
        public IEnumerable<FileInfoModel> SourceFiles { get; set; }
    }
}
