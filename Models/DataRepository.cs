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

        // Lecturer CRUD Methods
        public virtual IEnumerable<LecturerModel> GetAllLecturers()
        {
           
            return _context.Lecturers.OrderBy(l => l.FullName).ToList();
        }

        public virtual LecturerModel? GetLecturerById(string id)
        {
           
            return _context.Lecturers.Find(id);
        }

        public virtual void AddLecturer(LecturerModel lecturer)
        {
            _context.Lecturers.Add(lecturer);
            _context.SaveChanges();
        }

        public virtual void UpdateLecturer(LecturerModel updatedLecturer)
        {
            _context.Lecturers.Update(updatedLecturer);
            _context.SaveChanges(); 
        }

        public virtual void DeleteLecturer(string id)
        {
            var existing = GetLecturerById(id);
            if (existing != null)
            {
                _context.Lecturers.Remove(existing);
                _context.SaveChanges(); 
            }
        }

        // Claim CRUD Methods
        public virtual IEnumerable<ClaimModel> GetAllClaims()
        {
            return _context.Claims.OrderByDescending(c => c.SubmissionDate).ToList();
        }

        public virtual ClaimModel? GetClaimById(string id)
        {
            return _context.Claims.Find(id);
        }

        public virtual void AddClaim(ClaimModel claim)
        {
            _context.Claims.Add(claim);
            _context.SaveChanges();
        }

        public virtual void UpdateClaim(ClaimModel updatedClaim)
        {
            _context.Claims.Update(updatedClaim);
            _context.SaveChanges();
        }

        // Document CRUD Methods
        public virtual DocumentModel? GetDocumentById(string id)
        {
            return _context.Documents.Find(id);
        }

        public virtual void AddDocument(DocumentModel document)
        {
            _context.Documents.Add(document);
            _context.SaveChanges();
        }

        public virtual void UpdateDocument(DocumentModel updatedDocument)
        {
            _context.Documents.Update(updatedDocument);
            _context.SaveChanges();
        }
    }
}