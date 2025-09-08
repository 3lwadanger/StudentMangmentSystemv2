using StudentMangmentSystem.Models;
using StudentMangmentSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Services
{
    public class SuperAdminService : AdminService, ISuperAdminService
    {
        private readonly ISuperAdminRepository _superRepo;

        public SuperAdminService(ISuperAdminRepository repo) : base(repo)
        {
            _superRepo = repo;
        }

        public void AddUser()
        {
            int choice;
            while (true)
            {
                Console.WriteLine("Select user type:");
                Console.WriteLine("1 - Admin");
                Console.WriteLine("2 - Student");

                if (int.TryParse(Console.ReadLine(), out choice) && (choice == 1 || choice == 2))
                    break;
                else
                    Console.WriteLine("Invalid choice! Please enter 1 for Admin or 2 for Student.");
            }

            Console.WriteLine("Enter Name: ");
            string name = Console.ReadLine();

            string email;
            while (true)
            {
                Console.WriteLine("Enter Email: ");
                email = Console.ReadLine();

                if (IsValidEmail(email))
                    break;
                else
                    Console.WriteLine("Invalid email format! Try again.");
            }

            Console.WriteLine("Enter Password: ");
            string password = Console.ReadLine();

            if (choice == 1)
            {
                var admin = new Admin
                {
                    Name = name,
                    Email = email,
                    Password = password
                };
                _superRepo.AddAdmin(admin);
            }
            else
            {
                var student = new Student
                {
                    Name = name,
                    Email = email,
                    Password = password
                };
                _superRepo.AddStudent(student);
            }

            _superRepo.Save();
            Console.WriteLine($"{(choice == 1 ? "Admin" : "Student")} '{name}' added successfully!");
        }

        public void RemoveUser()
        {
            Console.WriteLine("Enter User Name/Email: ");
            string nameOrEmail = Console.ReadLine();

            var user = _superRepo.GetUserByNameOrEmail(nameOrEmail);
            if (user == null)
            {
                Console.WriteLine("No User Found!");
                return;
            }

            if (user.Token == 'A')
            {
                var admin = _superRepo.GetAdminByUserId(user.UserId);
                if (admin == null) { Console.WriteLine("ERROR! Couldn't find Admin profile."); return; }

                Console.WriteLine($"Are you sure you want to delete Admin '{admin.Name}'? (1 = Yes, 0 = No)");
                if (Console.ReadLine() == "1")
                {
                    _superRepo.RemoveUser(user);
                    _superRepo.Save();
                    Console.WriteLine($"Admin '{admin.Name}' removed successfully.");
                }
            }
            else if (user.Token == 'S')
            {
                var student = _superRepo.GetStudentByUserId(user.UserId);
                if (student == null) { Console.WriteLine("ERROR! Couldn't find Student profile."); return; }

                Console.WriteLine($"Are you sure you want to delete Student '{student.Name}'? (1 = Yes, 0 = No)");
                if (Console.ReadLine() == "1")
                {
                    _superRepo.RemoveUser(user);
                    _superRepo.Save();
                    Console.WriteLine($"Student '{student.Name}' removed successfully.");
                }
            }
            else
            {
                Console.WriteLine("Unknown user type.");
            }
        }
        public void ViewAllAdmins()
        {
            var admins = _superRepo.GetAllAdmins();

            Console.WriteLine("\n=== All Admins ===");
            foreach (var c in admins)
            {
                Console.WriteLine(c);
            }
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }

}
