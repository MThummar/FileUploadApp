using FileUploadApp.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace FileUploadApp.Pages
{
    public partial class FileUpload
    {
        // List to store uploaded files
        private List<FileUploadModel> _files = new();

        // Model for managing file upload data
        private readonly FileUploadModel _fileUploadModel = new();

        // Holds the currently selected file
        private IBrowserFile? _selectedFile;

        // Messages for user feedback
        private string _uploadMessage = string.Empty;
        private bool _isUploading = false;
        private string? _errorMessage;

        // Maximum file size limit (3 MB)
        private const long MaxFileSizeInBytes = 3 * 1024 * 1024;

        // This method initializes the component and loads file records
        protected override async Task OnInitializedAsync()
        {
            await LoadFileRecordsAsync();
        }

        // Fetches file records from the server
        private async Task LoadFileRecordsAsync()
        {
            try
            {
                _files = await FileService.GetFileRecordAsync();
            }
            catch (Exception ex)
            {
                _errorMessage = $"An error occurred while retrieving file records: {ex.Message}";
            }
        }

        // Handles file selection and provides feedback to the user
        private void OnFileSelected(InputFileChangeEventArgs e)
        {
            _uploadMessage = "";
            _errorMessage = "";
            _isUploading = false;
            _selectedFile = e.File;
        }

        // Uploads the selected file to the server and updates the file list
        private async Task UploadFileAsync()
        {
            // Check if a file has been selected
            if (_selectedFile == null)
            {
                _uploadMessage = "Please select a file to upload.";
                return;
            }

            // Validate the file size
            if (_selectedFile.Size > MaxFileSizeInBytes)
            {
                _errorMessage = "File size exceeds the 3 MB limit.";
                _selectedFile = null;
                return;
            }

            _isUploading = true;

            try
            {
                // Generate a unique file name and determine the path
                string fileName = GenerateUniqueFileName() + Path.GetExtension(_selectedFile.Name);
                string filePath = Path.Combine("Uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                // Save the file to the disk
                await SaveFileToDiskAsync(filePath);

                // Record the file details in the database
                await SaveFileRecordAsync(fileName, filePath);
                await LoadFileRecordsAsync();

                _uploadMessage = "File uploaded successfully.";
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error during file upload: {ex.Message}";
            }
            finally
            {
                _isUploading = false;
                _selectedFile = null;
            }
        }

        // Saves the uploaded file to the specified path
        private async Task SaveFileToDiskAsync(string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await _selectedFile.OpenReadStream().CopyToAsync(fileStream);
        }

        // Saves file details to the database
        private async Task SaveFileRecordAsync(string fileName, string filePath)
        {
            string extension = Path.GetExtension(_selectedFile.Name);
            DateTime lastModified = _selectedFile.LastModified.DateTime;

            await FileService.SaveFileRecordAsync(fileName, filePath, extension, DateTime.Now, lastModified);
        }

        // Generates a unique name for the file to avoid conflicts
        private static string GenerateUniqueFileName()
        {
            // Create a random file name without a dot
            return Path.GetRandomFileName().Replace(".", string.Empty).ToLower();
        }
    }
}
