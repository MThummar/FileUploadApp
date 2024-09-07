public interface IFileUploadService
{
    Task SaveFileRecordAsync(string fileName, string filePath, string extension, DateTime createdOn, DateTime lastModifiedOn);
    Task<List<FileUploadModel>> GetFileRecordAsync();
}

