using FileUploadApp.Models;

namespace FileUploadApp.Services;

/// <summary>
/// Defines the contract for file upload services, including methods for saving file records and retrieving them.
/// </summary>
public interface IFileUploadService
{
    /// <summary>
    /// Asynchronously saves a file record to the database.
    /// </summary>
    /// <param name="fileName">The name of the file being saved.</param>
    /// <param name="filePath">The path where the file is stored on the server.</param>
    /// <param name="extension">The file's extension (e.g., ".jpg", ".pdf").</param>
    /// <param name="createdOn">The date and time when the file was created.</param>
    /// <param name="lastModifiedOn">The date and time when the file was last modified.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when any of the arguments are invalid or empty.</exception>
    /// <exception cref="Exception">Thrown if an error occurs while saving the file record.</exception>
    Task SaveFileRecordAsync(string fileName, string filePath, string extension, DateTime createdOn, DateTime lastModifiedOn);

    /// <summary>
    /// Asynchronously retrieves all file records from the database and maps them to <see cref="FileUploadModel"/> instances.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="FileUploadModel"/> objects.</returns>
    /// <exception cref="Exception">Thrown if an error occurs while retrieving file records.</exception>
    Task<List<FileUploadModel>> GetFileRecordAsync();
}


