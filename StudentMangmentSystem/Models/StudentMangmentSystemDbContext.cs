using Microsoft.EntityFrameworkCore;
using StudentMangmentSystem.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentMangmentSystem.Models
{
    public class StudentMangmentSystemDbContext : DbContext
    {
        public StudentMangmentSystemDbContext(DbContextOptions<StudentMangmentSystemDbContext> options)
            : base(options)
        {
        }
        public StudentMangmentSystemDbContext()
        {
        }
        public DbSet<user> Users { get; set; }    
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentMangmentSystemDbContext).Assembly);

            modelBuilder.Entity<user>().ToTable("Users");
            modelBuilder.Entity<SuperAdmin>().ToTable("SuperAdmins");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments").HasKey(e => new { e.StudentId, e.CourseId }); ;
        }
    }

}