using Microsoft.AspNetCore.Components.Forms;

namespace FileUploadApp.Pages
{
    public partial class FileUpload
    {
        private List<FileUploadModel> _files = new();
        private FileUploadModel _fileUploadModel = new();
        private IBrowserFile? _selectedFile;
        private string _uploadMessage = string.Empty;
        private bool _isUploading = false;
        private string? _errorMessage;
        private const long MaxFileSizeInBytes = 3 * 1024 * 1024; // 3 MB

        protected override async Task OnInitializedAsync()
        {
            await LoadFileRecordsAsync();
        }

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

        private void OnFileSelected(InputFileChangeEventArgs e)
        {
            _selectedFile = e.File;
        }

        private async Task UploadFileAsync()
        {
            if (_selectedFile == null)
            {
                _uploadMessage = "No files selected.";
                return;
            }

            if (_selectedFile.Size > MaxFileSizeInBytes)
            {
                _errorMessage = "File size exceeds the maximum allowed size.";
                _selectedFile = null;
                return;
            }

            _isUploading = true;

            try
            {
                string fileName = GenerateUniqueFileName() + Path.GetExtension(_selectedFile.Name);
                string filePath = Path.Combine("Uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                await SaveFileToDiskAsync(filePath);

                await SaveFileRecordAsync(fileName, filePath);
                await LoadFileRecordsAsync();

                _uploadMessage = "File uploaded successfully.";
            }
            catch (Exception ex)
            {
                _errorMessage = $"Error uploading file: {ex.Message}";
                _selectedFile = null;
            }
            finally
            {
                _isUploading = false;
            }
        }

        private async Task SaveFileToDiskAsync(string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await _selectedFile.OpenReadStream().CopyToAsync(fileStream);
        }

        private async Task SaveFileRecordAsync(string fileName, string filePath)
        {
            string extension = Path.GetExtension(_selectedFile.Name);
            DateTime lastModified = _selectedFile.LastModified.DateTime;

            await FileService.SaveFileRecordAsync(fileName, filePath, extension, DateTime.Now, lastModified);
        }

        private static string GenerateUniqueFileName()
        {
            return Path.GetRandomFileName().Replace(".", string.Empty).ToLower();
        }
    }
}
