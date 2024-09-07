public class FileUploadModel
{
    // Default constructor
    public FileUploadModel() { }

    // Constructor with parameters for convenience
    public FileUploadModel(string name, string path, string extension, DateTime createdOn, DateTime lastModifiedOn)
    {
        Name = name;
        Path = path;
        Extension = extension;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the path where the file is stored.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the file extension.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the file was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the file was last modified.
    /// </summary>
    public DateTime LastModifiedOn { get; set; }
}
