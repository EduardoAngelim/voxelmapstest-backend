using System.Collections.Generic;
using VoxelMapsTestTask.Enums;

namespace VoxelMapsTestTask.Models
{
    public class FileHandleRequestModel
    {
        public FileHandleTypeEnum Type { get; set; }
        public string HubConnectionId { get; set; }
        public string DestinationFolder { get; set; }
        public List<string> FilesToHandle { get; set; }
    }
}
