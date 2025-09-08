using StudentMangmentSystem.Data;
using StudentMangmentSystem.Repositories;
using StudentMangmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public class SuperAdmin : user
    {


        public override void showMenu()
        {
            var db = new StudentMangmentSystemDbContext();
            var superService = new SuperAdminService(new SuperAdminRepository(db));

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Super Admin Menu ===");
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
                Console.WriteLine("11. Add New User");
                Console.WriteLine("12. Remove User");
                Console.WriteLine("13. View All Admins");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1": superService.EnrollStudent(); break;
                    case "2": superService.DropStudent(); break;
                    case "3": superService.ShowTop3Students(); break;
                    case "4":
                        Console.Write("Enter course code or title: ");
                        string keyword = Console.ReadLine();
                        superService.SearchCourse(keyword);
                        break;
                    case "5": superService.UpdateStudentGrade(); break;
                    case "6": superService.ViewUnenrolledStudents(); break;
                    case "7": superService.ViewCoursesByStudents(); break;
                    case "8": superService.ViewFamousCourses(); break;
                    case "9": superService.ViewAllStudents(); break;
                    case "10": superService.ViewAllCourses(); break;
                    case "11": superService.AddUser(); break;
                    case "12": superService.RemoveUser(); break;
                    case "13": superService.ViewAllAdmins(); break;
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
