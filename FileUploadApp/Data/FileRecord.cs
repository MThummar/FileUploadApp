namespace FileUploadApp.Data;


/// <summary>
/// Represents a record of a file in the database.
/// </summary>
public class FileRecord
{
    /// <summary>
    /// Gets or sets the unique identifier for the file record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the path where the file is stored.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the extension of the file.
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
