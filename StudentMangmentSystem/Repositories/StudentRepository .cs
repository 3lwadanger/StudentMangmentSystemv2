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
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentMangmentSystemDbContext _context;

        public StudentRepository(StudentMangmentSystemDbContext context)
        {
            _context = context;
        }

        public Student GetById(int id) =>
            _context.Students.FirstOrDefault(s => s.UserId == id);

        public IEnumerable<Enrollment> GetAllEnrollments(int studentId) =>
        _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .ToList();

        public IEnumerable<Course> GetEnrollments(int studentId) =>
            _context.Enrollments.AsNoTracking()
                .Where(e => e.StudentId == studentId)
                .Join(_context.Courses,
                      e => e.CourseId,   // Enrollment.CourseId
                      c => c.CourseId,         // Course.Id
                      (e, c) => c)       // return Course
                .ToList();



        public IEnumerable<Course> GetAllCourses() =>
            _context.Courses.AsNoTracking().ToList();

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

        public void Save() => _context.SaveChanges();
    }

}
