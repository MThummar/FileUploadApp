using FileUploadApp.Data;
using FileUploadApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApp.Services;

/// <summary>
/// Provides methods for handling file upload operations, including saving file records to the database and retrieving them.
/// </summary>
public class FileUploadService : IFileUploadService
{
    private readonly AppDbContext _context;
    private readonly ILogger<FileUploadService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileUploadService"/> class.
    /// </summary>
    /// <param name="context">The database context for accessing file records.</param>
    /// <param name="logger">The logger for logging error and informational messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> or <paramref name="logger"/> is null.</exception>
    public FileUploadService(AppDbContext context, ILogger<FileUploadService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Saves a file record to the database.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="filePath">The path where the file is stored.</param>
    /// <param name="extension">The file's extension.</param>
    /// <param name="createdOn">The date and time when the file was created.</param>
    /// <param name="lastModifiedOn">The date and time when the file was last modified.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown if an error occurs while saving the file record.</exception>
    public async Task SaveFileRecordAsync(string fileName, string filePath, string extension, DateTime createdOn, DateTime lastModifiedOn)
    {
        try
        {
            var fileRecord = new FileRecord
            {
                Name = fileName,
                Path = filePath,
                Extension = extension,
                CreatedOn = createdOn,
                LastModifiedOn = lastModifiedOn
            };

            _context.FileRecords.Add(fileRecord);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the file record.");
            throw; // Consider rethrowing to allow higher-level handling if necessary
        }
    }

    /// <summary>
    /// Retrieves all file records from the database and maps them to <see cref="FileUploadModel"/>.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="FileUploadModel"/> objects.</returns>
    /// <exception cref="Exception">Thrown if an error occurs while retrieving file records.</exception>
    public async Task<List<FileUploadModel>> GetFileRecordAsync()
    {
        var result = new List<FileUploadModel>();
        try
        {
            var fileRecords = await _context.FileRecords.ToListAsync();
            foreach (var file in fileRecords)
            {
                result.Add(new FileUploadModel
                {
                    Name = file.Name,
                    Path = file.Path,
                    Extension = file.Extension,
                    CreatedOn = file.CreatedOn,
                    LastModifiedOn = file.LastModifiedOn
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving file records.");
            throw; // Consider rethrowing to allow higher-level handling if necessary
        }
        return result;
    }
}

