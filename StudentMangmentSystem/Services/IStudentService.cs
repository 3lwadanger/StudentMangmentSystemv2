using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Services
{
    public interface IStudentService
    {
        void ViewCoursesEnrolled(int studentId);
        void ViewAllCourses();
        void ViewFamousCourses();
        void SearchCourse(string keyword);
        void ViewGpa(int studentId);
    }
}
