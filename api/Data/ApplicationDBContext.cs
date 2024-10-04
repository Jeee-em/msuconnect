using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base method for Identity configurations
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Mentor", NormalizedName = "MENTOR" },
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // Mentor for Subject
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Mentor)
                .WithMany(u => u.Expertise)
                .HasForeignKey(s => s.MentorId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascade deletions

            // Mentor for Session
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Mentor)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.MentorId)
                .OnDelete(DeleteBehavior.Restrict); // No cascade delete for Mentor

            // Student for Session
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // No cascade delete for Student

            // Subject for Session
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Subject)
                .WithMany()
                .HasForeignKey(s => s.SubjectId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete to prevent cascading issues with subjects

            // Feedback for Session
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Session)
                .WithMany()
                .HasForeignKey(f => f.SessionId)
                .OnDelete(DeleteBehavior.Cascade);  // Deleting sessions will cascade to feedbacks

            // Notification for User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting users will cascade to notifications
        }
    }
}
