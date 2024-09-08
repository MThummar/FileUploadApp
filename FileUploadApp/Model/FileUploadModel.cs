namespace FileUploadApp.Models;

public class FileUploadModel
{
    public FileUploadModel()
    {
    }

    public FileUploadModel(string name, string path, string extension, DateTime createdOn, DateTime lastModifiedOn)
    {
        Name = name;
        Path = path;
        Extension = extension;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    public string Name { get; set; }

    public string Path { get; set; }

    public string Extension { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime LastModifiedOn { get; set; }
}


