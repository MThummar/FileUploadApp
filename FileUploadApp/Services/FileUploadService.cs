using Microsoft.EntityFrameworkCore;

public class FileUploadService : IFileUploadService
{
    private readonly AppDbContext _context;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(AppDbContext context, ILogger<FileUploadService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SaveFileRecordAsync(string fileName, string filePath, string extension, DateTime createdOn, DateTime lastModifiedOn)
    {
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name cannot be empty.", nameof(fileName));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be empty.", nameof(filePath));
        if (string.IsNullOrWhiteSpace(extension)) throw new ArgumentException("File extension cannot be empty.", nameof(extension));

        try
        {
            var fileRecord = new FileRecords
            {
                Name = fileName,
                Path = filePath,
                Extention = extension,
                CreatedOn = createdOn,
                LastModifiedOn = lastModifiedOn
            };

            _context.FileRecords.Add(fileRecord);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the file record.");
        }
    }
    public async Task<List<FileUploadModel>> GetFileRecordAsync()
    {
        var result = new List<FileUploadModel>();
        try
        {
            var fileRecords = await _context.FileRecords.ToListAsync();
            foreach (var file in fileRecords)
            {
                result.Add(new FileUploadModel()
                {
                    Name = file.Name,
                    Path = file.Path,
                    Extension = file.Extention,
                    CreatedOn = file.CreatedOn,
                    LastModifiedOn = file.LastModifiedOn
                });
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving file records.");
        }
        return result;
    }
}

