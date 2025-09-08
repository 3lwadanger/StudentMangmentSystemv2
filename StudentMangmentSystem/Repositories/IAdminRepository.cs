using StudentMangmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Repositories
{
    public interface IAdminRepository
    {
        Student GetStudentByName(string name);
        Course GetCourseByTitleOrCode(string courseName);
        IEnumerable<Course> GetFamousCourses();
        Enrollment GetEnrollment(int studentId, int courseId);
        public Course GetCourseByCodeOrTitle(string keyword);
        void AddEnrollment(Enrollment enrollment);
        void RemoveEnrollment(Enrollment enrollment);
        List<Student> GetAllStudents();
        List<Course> GetAllCourses();
        List<Student> GetUnenrolledStudents();
        List<Enrollment> GetEnrollments();
        void Save();
    }

}
