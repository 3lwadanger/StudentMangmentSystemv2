using StudentMangmentSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace StudentMangmentSystem.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void ViewCoursesEnrolled(int studentId)
        {
            var enrollments = _studentRepository.GetEnrollments(studentId);

            Console.WriteLine("\n=== Courses Enrolled ===");
            foreach (var e in enrollments)
            {
                Console.WriteLine(e);
            }

            if (!enrollments.Any())
                Console.WriteLine("No enrollments found.");
        }

        public void ViewAllCourses()
        {
            var courses = _studentRepository.GetAllCourses();

            Console.WriteLine("\n=== All Courses ===");
            foreach (var c in courses)
            {
                Console.WriteLine(c);
            }
        }

        public void ViewFamousCourses()
        {
            var famousCourses = _studentRepository.GetFamousCourses();

            Console.WriteLine("\n=== Famous Courses (Top 3) ===");
            foreach (var c in famousCourses)
            {
                Console.WriteLine($"{c.Code} - {c.Title}");
            }
        }

        public void SearchCourse(string keyword)
        {
            var course = _studentRepository.GetCourseByCodeOrTitle(keyword);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"\nFound Course: {course.Code} - {course.Title}");
        }

        public void ViewGpa(int studentId)
        {
            var enrollments = _studentRepository.GetAllEnrollments(studentId)
                            .Select(e => new { e.CourseId, e.Grade })
                            .ToList();

            if (!enrollments.Any())
                WriteLine("No Courses Taken!");

            double totalGpa = 0;
            double actualCredits = 0;

            foreach (var e in enrollments)
            {
                var course = _studentRepository.GetAllCourses()
                            .FirstOrDefault(c => c.CourseId == e.CourseId);

                if (course != null)
                {
                    double courseCredits = course.Credits;
                    double gradeValue = e.Grade;

                    actualCredits += courseCredits;
                    totalGpa += (gradeValue / 100.0) * courseCredits;
                }
            }

            if (actualCredits == 0)
                WriteLine("GPA: 0");

            double gpa = (totalGpa / actualCredits) * 4;
            WriteLine($" GPA: {Math.Round(gpa, 2)}");
        }
    }

}
