using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Services
{
    public interface IAdminService
    {
        void EnrollStudent();
        void DropStudent();
        void ShowTop3Students();
        void UpdateStudentGrade();
        void ViewUnenrolledStudents();
        void ViewCoursesByStudents();
        void ViewFamousCourses();
        void SearchCourse(string keyword);
        void ViewAllStudents();
        void ViewAllCourses();
    }

}
