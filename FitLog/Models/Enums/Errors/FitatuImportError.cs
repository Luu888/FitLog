using FitLog.Attributes;

namespace FitLog.Models.Enums.Errors
{
    public enum FitatuImportError
    {
        [Toast("Not found")]
        NotFound = -1,

        [Toast("Import failed: entries for some dates already exist!")]
        AlreadyExists = -2,

        [Toast("No file was uploaded")]
        FileNotFound = -3,
    }
}
