using CMCS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CMCS.Models
{
    public class DataRepository
    {
        private readonly CmcsDbContext _context;

        public DataRepository(CmcsDbContext context)
        {
            _context = context;
        }

        // Retrieves all lecturers ordered by full name
        public virtual IEnumerable<LecturerModel> GetAllLecturers()
        {
            return _context.Lecturers.OrderBy(l => l.FullName).ToList();
        }

        // Fetches a single lecturer by unique identifier
        public virtual LecturerModel? GetLecturerById(string id)
        {
            return _context.Lecturers.Find(id);
        }

        // Inserts a new lecturer record into the database
        public virtual void AddLecturer(LecturerModel lecturer)
        {
            _context.Lecturers.Add(lecturer);
            _context.SaveChanges();
        }

        // Updates an existing lecturer record
        public virtual void UpdateLecturer(LecturerModel updatedLecturer)
        {
            _context.Lecturers.Update(updatedLecturer);
            _context.SaveChanges();
        }

        // Removes a lecturer from the system if found
        public virtual void DeleteLecturer(string id)
        {
            var existing = GetLecturerById(id);
            if (existing != null)
            {
                _context.Lecturers.Remove(existing);
                _context.SaveChanges();
            }
        }

        // Retrieves all claims sorted by submission date (newest first)
        public virtual IEnumerable<ClaimModel> GetAllClaims()
        {
            return _context.Claims.OrderByDescending(c => c.SubmissionDate).ToList();
        }

        // Fetches a specific claim by unique identifier
        public virtual ClaimModel? GetClaimById(string id)
        {
            return _context.Claims.Find(id);
        }

        // Adds a new claim entry to the database
        public virtual void AddClaim(ClaimModel claim)
        {
            _context.Claims.Add(claim);
            _context.SaveChanges();
        }

        // Updates an existing claim entry
        public virtual void UpdateClaim(ClaimModel updatedClaim)
        {
            _context.Claims.Update(updatedClaim);
            _context.SaveChanges();
        }

        // Retrieves a specific document by identifier
        public virtual DocumentModel? GetDocumentById(string id)
        {
            return _context.Documents.Find(id);
        }

        // Inserts a new document record
        public virtual void AddDocument(DocumentModel document)
        {
            _context.Documents.Add(document);
            _context.SaveChanges();
        }

        // Updates an existing document record
        public virtual void UpdateDocument(DocumentModel updatedDocument)
        {
            _context.Documents.Update(updatedDocument);
            _context.SaveChanges();
        }
    }
}
