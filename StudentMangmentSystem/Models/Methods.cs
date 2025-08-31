using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static System.Console;
using System.Net.Mail;

namespace StudentMangmentSystem.Models
{
    public class Methods
    {
        private readonly StudentMangmentSystemDbContext context;

        public Methods()
        {
            var options = new DbContextOptionsBuilder<StudentMangmentSystemDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=True;")
                .Options;

            context = new StudentMangmentSystemDbContext(options);
        }






        public void EnrollStudent()
        {
            WriteLine("Enter Student Name: ");
            string name = ReadLine();

            var student = context.Students
                .FirstOrDefault(s => s.Name.Contains(name));

            if (student == null)
            {
                WriteLine("Student Not Found!");
                return;
            }

            WriteLine("Enter Course Title or Code You Want To Enroll: ");
            string courseName = ReadLine();

            var course = context.Courses
                        .FirstOrDefault(c =>
                        c.Title.ToLower().Contains(courseName.ToLower()) ||
                        c.Code.ToLower().Contains(courseName.ToLower()));

            if (course == null)
            {
                WriteLine("Course Not Found!");
                return;
            }

            bool alreadyEnrolled = context.Enrollments
                .Any(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);

            if (alreadyEnrolled)
            {
                WriteLine("Student is already enrolled in this course!");
                return;
            }

            int grade;
            while (true)
            {
                WriteLine("Enter Grade (0-100): ");
                string gradeInput = ReadLine();

                if (int.TryParse(gradeInput, out grade) && grade >= 0 && grade <= 100)
                    break;
                else
                    WriteLine("Invalid grade. Please enter a number between 0 and 100.");
            }

            DateTime dateEnrolled;
            while (true)
            {
                WriteLine("Enter Enrollment Date (yyyy-MM-dd): ");
                string dateInput = ReadLine();

                if (DateTime.TryParse(dateInput, out dateEnrolled))
                    break;
                else
                    WriteLine("Invalid date format. Please enter in yyyy-MM-dd format.");
            }





            var enrollment = new Enrollment
            {
                StudentId = student.UserId,
                CourseId = course.CourseId,
                Grade = grade,
                DateEnrolled = DateTime.Now,
                
            };

            context.Enrollments.Add(enrollment);
            context.SaveChanges();
            WriteLine("Student Enrolled Successfully");

        }
        public void DropStudent()
        {
            WriteLine("Enter Student Name: ");
            string name = ReadLine();

            var student = context.Students
                .FirstOrDefault(s => s.Name.Contains(name));

            if (student == null)
            {
                WriteLine("Student Not Found!");
                return;
            }

            WriteLine("Enter Course Title or Code You Want To Drop: ");
            string courseName = ReadLine();

            var course = context.Courses
                        .FirstOrDefault(c =>
                        c.Title.ToLower().Contains(courseName.ToLower()) ||
                        c.Code.ToLower().Contains(courseName.ToLower()));

            if (course == null)
            {
                WriteLine("Course Not Found!");
                return;
            }

            bool alreadyEnrolled = context.Enrollments
                .Any(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);

            if (!alreadyEnrolled)
            {
                WriteLine("Student hasn't enrolled in this course!");
                return;
            }

            var enrollment = context.Enrollments
           .FirstOrDefault(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);
            context.Enrollments.Remove(enrollment);
            context.SaveChanges();
            WriteLine("Student Dropped Successfully");
        }
        public void GetTop3Students()
        {
            var studentGpas = context.Enrollments
                .AsNoTracking()
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
                    Course coursename = context.Courses
                        .FirstOrDefault(s => s.CourseId == scores.CourseId);

                    if (coursename != null)
                    {
                        double courseCredits = coursename.Credits;  
                        double gradeValue = scores.Grade;           

                        actualCredits += courseCredits;
                        totalGpa += gradeValue / 100.0 * courseCredits;
                    }
                }

                double score = totalGpa / actualCredits * 4 ;
                results.Add((student.Key, score));
            }

            var topStudents = results
                .OrderByDescending(e => e.Score)
                .Take(3)
                .ToList();

            foreach (var student in topStudents)
            {
                Student studentz = context.Students
                    .FirstOrDefault(e => e.UserId == student.StudentId);

                if (studentz != null)
                {
                    WriteLine($"{studentz}, GPA: {student.Score:F2}");
                }
            }
        }
        public void searchCourse()
        {
            WriteLine("Enter Course Title or Code: ");
            string courseName = ReadLine();

            var courses = context.Courses
           .AsNoTracking()
           .Where(c =>
               c.Title.ToLower().Contains(courseName.ToLower()) ||
               c.Code.ToLower().Contains(courseName.ToLower()))
           .ToList();


            if (!courses.Any())
            {
                WriteLine("No Courses Found!");
                return;
            }
            else
            {
                WriteLine(); WriteLine("Courses Found: ");
                foreach(var course in courses)
                {
                    WriteLine(course);
                }
            }



        }
        public void setGrade()
        {

            WriteLine("Enter Student Name: ");
            string name = ReadLine();

            var student = context.Students
                .FirstOrDefault(s => s.Name.Contains(name));

            if (student == null)
            {
                WriteLine("Student Not Found!");
                return;
            }

            WriteLine("Enter Course Title You Want To Assign Grade: ");
            string courseName = ReadLine();

            var course = context.Courses
                .FirstOrDefault(c => c.Title.Contains(courseName));

            if (course == null)
            {
                WriteLine("Course Not Found!");
                return;
            }

            bool alreadyEnrolled = context.Enrollments
                .Any(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);

            if (!alreadyEnrolled)
            {
                WriteLine("Student hasn't enrolled in this course!");
                return;
            }

            var enrollment = context.Enrollments
           .FirstOrDefault(e => e.StudentId == student.UserId && e.CourseId == course.CourseId);

            WriteLine("Enter The Grade To This Course: ");
            double Grade;
            while (!double.TryParse(ReadLine(), out Grade))
            {
                WriteLine("Invalid input. Please enter a valid number.");
            }
            enrollment.Grade = Grade;
            context.SaveChanges();

            WriteLine("Grades Updated successfully");


        }

        public void viewUnEnrolledStudents()
        {

            var studentsWithoutEnrollments = context.Students
                .Where(s => !context.Enrollments.Any(e => e.StudentId == s.UserId))
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
            var enrollmentList = context.Enrollments.AsNoTracking()
                .GroupBy(e => e.StudentId)
                .ToList();

            foreach (var studentGroup in enrollmentList) 
            {
                var studentz = context.Students.AsNoTracking()
                    .FirstOrDefault(s => s.UserId == studentGroup.Key);

                WriteLine(studentz);
                WriteLine("Courses Taken:");

                foreach (var enrollment in studentGroup)
                {
                    var course = context.Courses.AsNoTracking()
                        .FirstOrDefault(c => c.CourseId == enrollment.CourseId);

                    if (course != null)
                    {
                        WriteLine(course);
                    }
                }

                WriteLine(); 
            }
        }

        public void ViewFamousCourses()
        {
            var mostPopularCourse = context.Enrollments.AsNoTracking()
                .GroupBy(e => e.CourseId)
                .Select(g => new
                {
                    CourseId = g.Key,
                    StudentCount = g.Count()
                })
                .OrderByDescending(x => x.StudentCount)
                .Take(3).ToList();

            foreach(var coursez in mostPopularCourse) { 

            var course = context.Courses
            .FirstOrDefault(c => c.CourseId == coursez.CourseId);


            Write($"Course: {course}. ");
                    Write($"Students Enrolled: {coursez.StudentCount}");
                WriteLine();
                
            }
            
        }

        public void viewAllStudents()
        {

            var students = context.Students.AsNoTracking().ToList();
            foreach (var student in students) { WriteLine(student); }
        }
        public void viewAllCourses()
        {
            var courses = context.Courses.AsNoTracking().ToList();
            foreach (var course in courses) { WriteLine(course); }
        }



        public void ViewCoursesTaken(int ID)
        {
            var courseIds = context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == ID)
                .Select(e => e.CourseId)
                .ToList();

            if (courseIds.Any())
            {
                var courses = context.Courses
                    .Where(c => courseIds.Contains(c.CourseId))
                    .ToList();
                WriteLine("Courses being taken:");WriteLine();
                foreach (var course in courses)
                {
                    WriteLine(course);
                }
            }
            else
            {
                Console.WriteLine("No Courses Enrolled");
            }
        }

        public void viewGpa(int ID)
        {

       
        
            var enrollments = context.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == ID)
                .Select(e => new { e.CourseId, e.Grade })
                .ToList();

            if (!enrollments.Any())
                WriteLine("No Courses Taken!");

            double totalGpa = 0;
            double actualCredits = 0;

            foreach (var e in enrollments)
            {
                var course = context.Courses
                    .AsNoTracking()
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





    
