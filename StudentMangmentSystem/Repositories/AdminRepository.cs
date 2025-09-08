using Microsoft.EntityFrameworkCore;
using StudentMangmentSystem.Data;
using StudentMangmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly StudentMangmentSystemDbContext _context;

        public AdminRepository(StudentMangmentSystemDbContext context)
        {
            _context = context;
        }

        public Student GetStudentByName(string name) =>
            _context.Students.FirstOrDefault(s => s.Name.Contains(name));

        public Course GetCourseByTitleOrCode(string courseName) =>
            _context.Courses.FirstOrDefault(c =>
                c.Title.ToLower().Contains(courseName.ToLower()) ||
                c.Code.ToLower().Contains(courseName.ToLower()));

        public Enrollment GetEnrollment(int studentId, int courseId) =>
            _context.Enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);

        public void AddEnrollment(Enrollment enrollment) => _context.Enrollments.Add(enrollment);

        public void RemoveEnrollment(Enrollment enrollment) => _context.Enrollments.Remove(enrollment);

        public List<Student> GetAllStudents() => _context.Students.AsNoTracking().ToList();

        public List<Course> GetAllCourses() => _context.Courses.AsNoTracking().ToList();

        public List<Student> GetUnenrolledStudents() =>
            _context.Students
                .Where(s => !_context.Enrollments.Any(e => e.StudentId == s.UserId))
                .ToList();
        public IEnumerable<Course> GetFamousCourses() =>
            _context.Enrollments
                .GroupBy(e => e.CourseId)
                .Select(g => new { CourseId = g.Key, Count = g.Count() }) // project into new object
                .OrderByDescending(g => g.Count)
                .Take(3)
                .Join(_context.Courses,
                      g => g.CourseId,
                      c => c.CourseId,   // make sure your Course PK is Id
                      (g, c) => c)
                .ToList();

        public Course GetCourseByCodeOrTitle(string keyword) =>
            _context.Courses.FirstOrDefault(c =>
                c.Code.Contains(keyword) || c.Title.Contains(keyword));
        public List<Enrollment> GetEnrollments() =>
            _context.Enrollments.ToList();

        public void Save() => _context.SaveChanges();
    }

}
