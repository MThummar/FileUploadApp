using System.ComponentModel.DataAnnotations;
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
    [Required]
    [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters.")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the path where the file is stored.
    /// </summary>
    [Required]
    [StringLength(1000, ErrorMessage = "File path cannot exceed 1000 characters.")]
    public required string Path { get; set; }

    /// <summary>
    /// Gets or sets the extension of the file.
    /// </summary>
    [Required]
    [StringLength(10, ErrorMessage = "File extension cannot exceed 10 characters.")]
    public required string Extension { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the file was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the file was last modified.
    /// </summary>
    public DateTime LastModifiedOn { get; set; }

    
}
