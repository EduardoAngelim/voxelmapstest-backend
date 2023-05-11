using System.ComponentModel.DataAnnotations;

namespace VoxelMapsTestTask.RequestHandle
{
    public enum RequestAnswer
    {
        #region File

        [Display(Description = "Operation completed successfully!.")]
        FileHandleSuccess,

        [Display(Description = "Error when trying to conclude the operation.")]
        FileHandleError,
                
        [Display(Description = "Error when trying to get the files and directories.")]
        FileListError,

        [Display(Description = "Operation canceled successfully!")]
        FileHandleCanceledSuccess,

        [Display(Description = "Error when trying to cancel operation.")]
        FileHandleCanceledError,

        #endregion
    }
}