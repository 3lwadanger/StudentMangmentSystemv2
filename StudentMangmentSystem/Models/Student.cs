using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public class Student : user
    {

        public override void showMenu()
        {
            var methods = new Methods();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Student Menu ===");
                Console.WriteLine("1. View Courses Enrolled");
                Console.WriteLine("2. View All Courses");
                Console.WriteLine("3. View Famous Courses");
                Console.WriteLine("4. Search Course"); ;
                Console.WriteLine("5. View GPA"); ;
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        methods.ViewCoursesTaken(UserId);
                        break;

                    case "2":
                        methods.viewAllCourses();

                        break;

                    case "3":

                        methods.ViewFamousCourses();

                        break;

                    case "4":

                        methods.searchCourse();

                        break;

                    case "5":

                        methods.viewGpa(UserId);
                        break;


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
