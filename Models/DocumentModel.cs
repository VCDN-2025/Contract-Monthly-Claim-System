using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    /// <summary>
    /// Represents the metadata for a supporting document uploaded by a lecturer.
    /// </summary>
    public class DocumentModel
    {
        //System Identification Fields
        // Property: Unique identifier for the document, auto-generated upon creation.
        [Key]
        [Required]
        public string DocumentId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

        // Property: Foreign key linking this document back to the associated claim.
        [Required]
        public string ClaimId { get; set; } = string.Empty;

        //File Metadata Fields
        // Property: The file name provided by the user during upload.
        [Required(ErrorMessage = "File name is required.")]
        [Display(Name = "Original File Name")]
        public string OriginalFileName { get; set; } = string.Empty;

        // Property: The date and time the document was uploaded.
        [Required]
        [Display(Name = "Upload Date")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        // Property: The secure, encrypted path where the file is stored on the server.
        [Required(ErrorMessage = "Encrypted storage path is required.")]
        [Display(Name = "Encrypted File Path")]

        //This is the path on the disk where the encrypted file will be stored
        public string EncryptedFilePath { get; set; } = string.Empty;

        // Property: The size of the file in bytes, must be greater than zero.
        [Required]
        [Display(Name = "File Size (Bytes)")]
        [Range(1, long.MaxValue, ErrorMessage = "File size must be greater than zero.")]
        public long FileSize { get; set; }
    }
}