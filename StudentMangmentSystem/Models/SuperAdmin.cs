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
            var methods = new Methods();
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
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        methods.EnrollStudent();
                        break;

                    case "2":
                        methods.DropStudent();

                        break;

                    case "3":

                        methods.GetTop3Students();

                        break;

                    case "4":

                        methods.searchCourse();

                        break;

                    case "5":
                        methods.setGrade();

                        break;

                    case "6":

                        methods.viewUnEnrolledStudents();
                        break;

                    case "7":
                        methods.ViewCoursesByStudents();
                        break;
                    case "8":
                        methods.ViewFamousCourses();
                        break;
                    case "9":
                        methods.viewAllStudents();
                        break;
                    case "10":
                        methods.viewAllCourses();
                        break;
                    case "11":
                        methods.addUser();
                        break;
                    case "12":
                        methods.removeUser(); break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            
             }
        }
        public override string ToString()
        {
            return $"ID: {UserId}, Name: {Name}, Email: {Email}";
        }
    }
}
