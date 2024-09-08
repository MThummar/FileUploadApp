using FileUploadApp.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace FileUploadApp.Pages
{
    public partial class FileUpload
    {

        // List to store uploaded files
        private List<FileUploadModel> _uoloadedFiles = new();

        // Model for managing file upload data
        private readonly FileUploadModel _fileUploadModel = new();

        // Holds the currently selected file
        private IBrowserFile? _selectedFile;

        // Messages for user feedback
        private string? _uploadMessage;
        private bool _isUploading;
        private string? _errorMessage;

        // This method initializes the component and loads file records
        protected override async Task OnInitializedAsync()
        {
            await LoadFileRecordsAsync();
        }

        // Handles file selection and provides feedback to the user
        private void OnFileSelected(InputFileChangeEventArgs e)
        {
            ResetFileUploadState();
            _selectedFile = e.File;
        }

        // Uploads the selected file to the server and updates the file list
        private async Task UploadFileAsync()
        {
            try
            {
                // Validate if File is Selected and the size of the selected file is less than maximum allowed file size limit.
                if (FileIsNotSelected()) return;
                if (FileSizeIsNotInSpecifiedFileLimit()) return;

                _isUploading = true;

                string destinationDirectoryPath = configuration.GetValue<string>("FILE_UPLOAD_DESTINATION_DIR_PATH")!;
                // Generate a unique file name and determine the path

                string fileName = GenerateUniqueFileName() + Path.GetExtension(_selectedFile.Name);
                string filePath = Path.Combine(destinationDirectoryPath, fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                // Save the file to the disk
                await SaveFileToDiskAsync(filePath);

                // Record the file details in the database
                await SaveFileRecordAsync(fileName, filePath);

                // Load file record from database
                await LoadFileRecordsAsync();

                _isUploading = false;
                _uploadMessage = "File uploaded successfully.";

            }
            catch (Exception ex)
            {
                ResetFileUploadState();
                _errorMessage = $"Error during file upload: {ex.Message}";
            }
            finally
            {
                //ResetFileUploadState();
            }
        }

        // Fetches file records from the server
        private async Task LoadFileRecordsAsync()
        {
            try
            {
                _uoloadedFiles = await FileService.GetFileRecordAsync();
            }
            catch (Exception ex)
            {
                _errorMessage = $"An error occurred while retrieving file records: {ex.Message}";
            }
        }



        #region Helper Methods

        // Check if a file has been selected
        private bool FileIsNotSelected()
        {

            if (_selectedFile == null)
            {
                _uploadMessage = "Please select a file to upload.";
                return true;
            }
            return false;
        }

        // Validate the file size
        private bool FileSizeIsNotInSpecifiedFileLimit()
        {
            var maxAllowedSize = configuration.GetValue<long>("MAX_ALLOWED_FILE_UPLOAD_SIZE");
            if (_selectedFile?.Size > maxAllowedSize)
            {
                _errorMessage = "File size exceeds the 3 MB limit.";
                return true;
            }
            return false;
        }

        // Generates a unique name for the file to avoid conflicts
        private static string GenerateUniqueFileName()
        {
            // Create a random file name without a dot
            return Path.GetRandomFileName().Replace(".", string.Empty).ToLower();
        }

        // Reset state before starting new upload
        private void ResetFileUploadState()
        {
            _uploadMessage = string.Empty;
            _errorMessage = string.Empty;
            _isUploading = false;
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

        #endregion
    }
}
