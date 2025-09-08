using StudentMangmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Repositories
{
    public interface IStudentRepository
    {
        Student GetById(int id);
        public IEnumerable<Enrollment> GetAllEnrollments(int studentId);
        IEnumerable<Course> GetEnrollments(int studentId);
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Course> GetFamousCourses();
        Course GetCourseByCodeOrTitle(string keyword);
        void Save();
    }
}
