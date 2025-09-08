using StudentMangmentSystem.Data;
using StudentMangmentSystem.Repositories;
using StudentMangmentSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public class Admin : user
    {

        public override void showMenu()
        {
            var db = new StudentMangmentSystemDbContext();
            var adminService = new AdminService(new AdminRepository(db));

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Admin Menu ===");
                Console.WriteLine("1. Enroll Student");
                Console.WriteLine("2. Drop Student");
                Console.WriteLine("3. Show Top 3 Students");
                Console.WriteLine("4. Search Course");
                Console.WriteLine("5. Update Student Grade");
                Console.WriteLine("6. View Students Without Enrollments");
                Console.WriteLine("7. View Courses By Student");
                Console.WriteLine("8. View Famous Courses");
                Console.WriteLine("9. View All Students");
                Console.WriteLine("10. View All Courses");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1": adminService.EnrollStudent(); break;
                    case "2": adminService.DropStudent(); break;
                    case "3": adminService.ShowTop3Students(); break;
                    case "4":
                        Console.Write("Enter course code or title: ");
                        string keyword = Console.ReadLine();
                        adminService.SearchCourse(keyword);
                        break;
                    case "5": adminService.UpdateStudentGrade(); break;
                    case "6": adminService.ViewUnenrolledStudents(); break;
                    case "7": adminService.ViewCoursesByStudents(); break;
                    case "8": adminService.ViewFamousCourses(); break;
                    case "9": adminService.ViewAllStudents(); break;
                    case "10": adminService.ViewAllCourses(); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Invalid choice. Try again."); break;
                }
            }
        }

        public override string ToString()
        {
            return $"ID: {UserId}, Name: {Name}, Email: {Email}";
        }

    }
}
