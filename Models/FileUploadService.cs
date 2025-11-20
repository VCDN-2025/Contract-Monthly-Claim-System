using CMCS.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMCS.Services
{
    /// <summary>
    /// Handles the validation and secure storage of uploaded supporting documents.
    /// </summary>
    public class FileUploadService
    {
        // Constant: Maximum allowed file size for uploads (5 MB).
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
        // Static Field: Array of file extensions allowed for upload.
        private static readonly string[] AllowedExtensions = { ".pdf", ".docx", ".xlsx" };
        // Constant: Name of the secure directory where files are stored.
        private const string UploadsFolder = "SecureUploads";

        // Constructor: Ensures the secure uploads directory exists upon service initialization.
        public FileUploadService()
        {
            if (!Directory.Exists(UploadsFolder))
            {
                Directory.CreateDirectory(UploadsFolder);
            }
        }


        // Method: Validates the file, saves it securely to disk, and creates the DocumentModel metadata.
        public virtual async Task<(bool Success, string Message, DocumentModel? Document)> ProcessUploadAsync(IFormFile? file, string claimId)
        {

            try
            {
                if (file == null || file.Length == 0)
                {
                    return (false, "Please select a file to upload.", null);
                }

                // File Size Validation
                if (file.Length > MaxFileSizeInBytes)
                {
                    return (false, $"File size exceeds the limit of {MaxFileSizeInBytes / (1024 * 1024)} MB.", null);
                }

                // File Type Validation
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !AllowedExtensions.Contains(fileExtension))
                {
                    return (false, $"Invalid file type. Only {string.Join(", ", AllowedExtensions)} are allowed.", null);
                }

                // Secure Storage Setup
                var uniqueFileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
                var filePath = Path.Combine(UploadsFolder, uniqueFileName);

                // Saves the file to the secure folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Creates Document Metadata Model
                var documentModel = new DocumentModel
                {
                    ClaimId = claimId,
                    OriginalFileName = file.FileName,
                    EncryptedFilePath = filePath,
                    FileSize = file.Length
                };

                return (true, "File uploaded successfully.", documentModel);
            }
            catch (Exception ex)
            {
                return (false, $"A critical server error occurred during file processing: {ex.Message}", null);
            }
        }
    }
}