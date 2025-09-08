using StudentMangmentSystem.Models;
using StudentMangmentSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace StudentMangmentSystem.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public void EnrollStudent()
        {
            Console.WriteLine("Enter Student Name: ");
            string name = Console.ReadLine();
            var student = _adminRepository.GetStudentByName(name);

            if (student == null)
            {
                Console.WriteLine("Student Not Found!");
                return;
            }

            Console.WriteLine("Enter Course Title or Code You Want To Enroll: ");
            string courseName = Console.ReadLine();
            var course = _adminRepository.GetCourseByTitleOrCode(courseName);

            if (course == null)
            {
                Console.WriteLine("Course Not Found!");
                return;
            }

            if (_adminRepository.GetEnrollment(student.UserId, course.CourseId) != null)
            {
                Console.WriteLine("Student is already enrolled in this course!");
                return;
            }

            Console.WriteLine("Enter Grade (0-100): ");
            int grade = int.Parse(Console.ReadLine());

            var enrollment = new Enrollment
            {
                StudentId = student.UserId,
                CourseId = course.CourseId,
                Grade = grade,
                DateEnrolled = DateTime.Now
            };

            _adminRepository.AddEnrollment(enrollment);
            _adminRepository.Save();

            Console.WriteLine("Student Enrolled Successfully");
        }

        public void DropStudent()
        {
            Console.WriteLine("Enter Student Name: ");
            string name = Console.ReadLine();
            var student = _adminRepository.GetStudentByName(name);

            if (student == null)
            {
                Console.WriteLine("Student Not Found!");
                return;
            }

            Console.WriteLine("Enter Course Title or Code You Want To Drop: ");
            string courseName = Console.ReadLine();
            var course = _adminRepository.GetCourseByTitleOrCode(courseName);

            if (course == null)
            {
                Console.WriteLine("Course Not Found!");
                return;
            }

            var enrollment = _adminRepository.GetEnrollment(student.UserId, course.CourseId);
            if (enrollment == null)
            {
                Console.WriteLine("Student hasn't enrolled in this course!");
                return;
            }

            _adminRepository.RemoveEnrollment(enrollment);
            _adminRepository.Save();
            Console.WriteLine("Student Dropped Successfully");
        }

        public void ShowTop3Students()
        {
            var studentGpas = _adminRepository.GetEnrollments()
                .Select(e => new { e.StudentId, e.CourseId, e.Grade })
                .GroupBy(e => e.StudentId)
                .ToList();

            var results = new List<(int StudentId, double Score)>();

            foreach (var student in studentGpas)
            {
                double totalGpa = 0;
                double actualCredits = 0;

                foreach (var scores in student)
                {
                    Course coursename = _adminRepository.GetAllCourses()
                        .FirstOrDefault(s => s.CourseId == scores.CourseId);

                    if (coursename != null)
                    {
                        double courseCredits = coursename.Credits;
                        double gradeValue = scores.Grade;

                        actualCredits += courseCredits;
                        totalGpa += gradeValue / 100.0 * courseCredits;
                    }
                }

                double score = totalGpa / actualCredits * 4;
                results.Add((student.Key, score));
            }

            var topStudents = results
                .OrderByDescending(e => e.Score)
                .Take(3)
                .ToList();

            foreach (var student in topStudents)
            {
                Student studentz = _adminRepository.GetAllStudents()
                    .FirstOrDefault(e => e.UserId == student.StudentId);

                if (studentz != null)
                {
                    WriteLine($"{studentz}, GPA: {student.Score:F2}");
                }
            }
        }
        public void SearchCourse(string keyword) {
            var course = _adminRepository.GetCourseByCodeOrTitle(keyword);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"\nFound Course: {course.Code} - {course.Title}");
        }
        public void UpdateStudentGrade() {
            WriteLine("Enter Student Name: ");
            string name = ReadLine();

            var student = _adminRepository.GetAllStudents()
                .FirstOrDefault(s => s.Name.Contains(name));

            if (student == null)
            {
                WriteLine("Student Not Found!");
                return;
            }

            WriteLine("Enter Course Title You Want To Assign Grade: ");
            string courseName = ReadLine();

            var course = _adminRepository.GetAllCourses()
                .FirstOrDefault(c => c.Title.Contains(courseName));

            if (course == null)
            {
                WriteLine("Course Not Found!");
                return;
            }

            bool alreadyEnrolled = _adminRepository.GetEnrollments()
                .Any(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);

            if (!alreadyEnrolled)
            {
                WriteLine("Student hasn't enrolled in this course!");
                return;
            }

            var enrollment = _adminRepository.GetEnrollments()
           .FirstOrDefault(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);

            WriteLine("Enter The Grade To This Course: ");
            double Grade;
            while (!double.TryParse(ReadLine(), out Grade))
            {
                WriteLine("Invalid input. Please enter a valid number.");
            }
            enrollment.Grade = Grade;
            _adminRepository.Save();

            WriteLine("Grades Updated successfully");
        }
        public void ViewUnenrolledStudents() {
            var studentsWithoutEnrollments = _adminRepository.GetAllStudents()
                .Where(s => !_adminRepository.GetEnrollments().Any(e => e.StudentId == s.UserId))
                .ToList();
            if (studentsWithoutEnrollments.Any())
            {

                WriteLine("Students Without Enrollments: ");
                foreach (var student in studentsWithoutEnrollments)
                {

                    WriteLine(student);

                }
            }
            else { WriteLine("No Students Found"); }
        }
        public void ViewCoursesByStudents()
        {
            var enrollmentList = _adminRepository.GetEnrollments()
                .GroupBy(e => e.StudentId)
                .ToList();

            foreach (var studentGroup in enrollmentList)
            {
                var studentz = _adminRepository.GetAllStudents()
                    .FirstOrDefault(s => s.UserId == studentGroup.Key);

                WriteLine(studentz);
                WriteLine("Courses Taken:");

                foreach (var enrollment in studentGroup)
                {
                    var course = _adminRepository.GetAllCourses()
                        .FirstOrDefault(c => c.CourseId == enrollment.CourseId);

                    if (course != null)
                    {
                        WriteLine(course);
                    }
                }

                WriteLine();
            }
        }


        public void ViewFamousCourses() {
            var famousCourses = _adminRepository.GetFamousCourses();

            Console.WriteLine("\n=== Famous Courses (Top 3) ===");
            foreach (var c in famousCourses)
            {
                Console.WriteLine($"{c.Code} - {c.Title}");
            }
        }
        public void ViewAllStudents() {
            var courses = _adminRepository.GetAllStudents();

            Console.WriteLine("\n=== All Students ===");
            foreach (var c in courses)
            {
                Console.WriteLine(c);
            }
        }
        public void ViewAllCourses() {
            var courses = _adminRepository.GetAllCourses();

            Console.WriteLine("\n=== All Courses ===");
            foreach (var c in courses)
            {
                Console.WriteLine(c);
            }
        }

       
    }

}
