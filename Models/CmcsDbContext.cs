using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Models
{
    // Inheriting from IdentityDbContext to integrate ASP.NET Identity tables
    public class CmcsDbContext : IdentityDbContext
    {
        public CmcsDbContext(DbContextOptions<CmcsDbContext> options)
            : base(options)
        {
        }

    
        public DbSet<ClaimModel> Claims { get; set; }
        public DbSet<LecturerModel> Lecturers { get; set; }
        public DbSet<DocumentModel> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
            builder.Entity<LecturerModel>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(l => l.IdentityUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}