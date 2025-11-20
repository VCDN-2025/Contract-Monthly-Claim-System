using System.Text.Json;
using CMCS.Models;
using CMCS.Security;
namespace CMCS.Data
{
    /// <summary>
    /// Manages secure in-memory storage, loading, and persistence (via encryption) of all application data.
    /// </summary>
    public class DataRepository
    {
        // Constant: Path to the secure, encrypted file storage.
        private const string DataFilePath = "claims_data_secure.json";

        // Field: In-memory list storing all ClaimModel objects.
        private List<ClaimModel> _claims;
        // Field: In-memory list storing all DocumentModel objects.
        private List<DocumentModel> _documents;
        // Field: In-memory list storing all LecturerModel objects.
        private List<LecturerModel> _lecturers;

        // Constructor: Initializes the lists and attempts to load existing data from the file.
        public DataRepository()
        {

            _claims = new List<ClaimModel>();
            _documents = new List<DocumentModel>();
            _lecturers = new List<LecturerModel>();

            LoadData();
        }


        // Method: Attempts to read, decrypt, and deserialize data from the secure file path into memory.
        private void LoadData()
        {
            if (!File.Exists(DataFilePath))
            {
                // Safe return as lists are already initialized to empty lists.
                return;
            }

            try
            {
                // Reads the encrypted content from the file
                string encryptedJson = File.ReadAllText(DataFilePath);

                // Decrypts the content
                string decryptedJson = SecurityHelper.Decrypt(encryptedJson);

                // Deserializes the JSON back into our data structure
                var data = JsonSerializer.Deserialize<DataStructure>(decryptedJson);

                if (data != null)
                {

                    _claims = data.Claims ?? new List<ClaimModel>();
                    _documents = data.Documents ?? new List<DocumentModel>();
                    _lecturers = data.Lecturers ?? new List<LecturerModel>();
                }
            }
            catch (Exception ex)
            {
                // Handles potential decryption or file access errors
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        // Method: Serializes all in-memory data, encrypts it, and writes it to the secure file path.
        public void SaveData()
        {
            try
            {
                var data = new DataStructure
                {
                    Claims = _claims,
                    Documents = _documents,
                    Lecturers = _lecturers // Include the Lecturer data
                };

                // Serializes and Encrypts
                string plainJson = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                string encryptedJson = SecurityHelper.Encrypt(plainJson);

                // Writes the encrypted string to the file
                File.WriteAllText(DataFilePath, encryptedJson);
            }
            catch (Exception ex)
            {
                // Handles serialization or file writing errors
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        // Lecturer CRUD Methods
        // Method: Returns an ordered collection of all lecturer records.
        public virtual IEnumerable<LecturerModel> GetAllLecturers()
        {
            return _lecturers.OrderBy(l => l.FullName);
        }

        // Method: Finds and returns a specific lecturer record by their unique ID.
        public virtual LecturerModel? GetLecturerById(string id)
        {
            return _lecturers.FirstOrDefault(l => l.LecturerId == id);
        }

        // Method: Adds a new lecturer record to the in-memory list and persists the data.
        public virtual void AddLecturer(LecturerModel lecturer)
        {
            _lecturers.Add(lecturer);
            SaveData(); // Persist changes
        }

        // Method: Replaces an existing lecturer record with the updated model and persists the data.
        public virtual void UpdateLecturer(LecturerModel updatedLecturer)
        {
            var existing = GetLecturerById(updatedLecturer.LecturerId);
            if (existing != null)
            {
                // Simple update logic
                _lecturers.Remove(existing);
                _lecturers.Add(updatedLecturer);
                SaveData();
            }
        }

        // Method: Removes a specific lecturer record by ID from the in-memory list and persists the data.
        public virtual void DeleteLecturer(string id)
        {
            var existing = GetLecturerById(id);
            if (existing != null)
            {
                _lecturers.Remove(existing);
                SaveData();
            }
        }

        // Claim CRUD Methods
        // Method: Returns a collection of all claims, ordered by the submission date descending.
        public virtual IEnumerable<ClaimModel> GetAllClaims()
        {
            return _claims.OrderByDescending(c => c.SubmissionDate);
        }

        // Method: Finds and returns a specific claim record by its unique ID.
        public virtual ClaimModel? GetClaimById(string id)
        {
            return _claims.FirstOrDefault(c => c.ClaimId == id);
        }

        // Method: Adds a new claim record to the in-memory list and persists the data.
        public virtual void AddClaim(ClaimModel claim)
        {
            _claims.Add(claim);
            SaveData();
        }

        // Method: Replaces an existing claim record with the updated model and persists the data.
        public virtual void UpdateClaim(ClaimModel updatedClaim)
        {
            var existing = GetClaimById(updatedClaim.ClaimId);
            if (existing != null)
            {
                // Update logic
                _claims.Remove(existing);
                _claims.Add(updatedClaim);
                SaveData();
            }
        }

        // Document CRUD Methods
        // Method: Finds and returns a specific document record by its unique ID.
        public virtual DocumentModel? GetDocumentById(string id)
        {
            return _documents.FirstOrDefault(d => d.DocumentId == id);
        }

        // Method: Adds a new document record to the in-memory list and persists the data.
        public virtual void AddDocument(DocumentModel document)
        {
            _documents.Add(document);
            SaveData();
        }

        // Method: Replaces an existing document record with the updated model and persists the data.
        public virtual void UpdateDocument(DocumentModel updatedDocument)
        {
            var existing = GetDocumentById(updatedDocument.DocumentId);
            if (existing != null)
            {
                _documents.Remove(existing);
                _documents.Add(updatedDocument);
                SaveData();
            }
        }
    }


    // Internal Class: Defines the top-level structure for data serialization and deserialization.
    internal class DataStructure
    {

        // Property: List of all ClaimModel records.
        public List<ClaimModel>? Claims { get; set; }
        // Property: List of all DocumentModel records.
        public List<DocumentModel>? Documents { get; set; }
        // Property: List of all LecturerModel records.
        public List<LecturerModel>? Lecturers { get; set; }
    }
}