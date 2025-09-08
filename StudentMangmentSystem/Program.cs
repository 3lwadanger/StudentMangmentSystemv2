using Microsoft.EntityFrameworkCore;
using StudentMangmentSystem.Data;
using StudentMangmentSystem.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;
using System.Text.RegularExpressions;


var options = new DbContextOptionsBuilder<StudentMangmentSystemDbContext>()
           .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=True;")
           .Options;


using (var context = new StudentMangmentSystemDbContext(options))
{

    DbInitializer.Seed(context);

    bool running = true;

    while (running)
    {
        Console.WriteLine("\n--- Main Menu ---");
        Console.WriteLine("1. Login");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                LoginMenu(); // Call the login menu function
                break;

            case "0":
                running = false;
                Console.WriteLine("Exiting... Goodbye!");
                break;

            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }


    void LoginMenu()
    {
        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        // Validate email format before accessing DB
        if (!IsValidEmail(email))
        {
            Console.WriteLine("Invalid email format! Please try again.");
            return;
        }

        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        // Find user in DB
        var user = context.Users
            .FirstOrDefault(u =>
                EF.Functions.Collate(u.Email, "SQL_Latin1_General_CP1_CS_AS") == email &&
                EF.Functions.Collate(u.Password, "SQL_Latin1_General_CP1_CS_AS") == password);

        if (user == null)
        {
            Console.WriteLine("Invalid email or password.");
            return;
        }

        // Check token
        if (user.Token == 'A')
        {
            var admin = context.Admins
                .FirstOrDefault(a => a.UserId == user.UserId);

            if (admin != null)
            {
                Console.WriteLine($"Welcome Admin {admin.Name}!");
                admin.showMenu();
            }
            else
            {
                Console.WriteLine("ERROR! couldn't find Admin profile.");
            }
        }
        else if (user.Token == 'S')
        {
            var student = context.Students
                .FirstOrDefault(s => s.UserId == user.UserId);

            if (student != null)
            {
                Console.WriteLine($"Welcome Student {student.Name}!");
                student.showMenu();
            }
            else
            {
                Console.WriteLine("ERROR! couldn't find Student profile.");
            }
        }
        else if (user.Token == 'O')
        {
            var superAdmin = context.SuperAdmins
                .FirstOrDefault(s => s.UserId == user.UserId);

            if (superAdmin != null)
            {
                Console.WriteLine($"Welcome Super Admin {superAdmin.Name}!");
                superAdmin.showMenu();
            }
            else
            {
                Console.WriteLine("ERROR! couldn't find Super Admin profile.");
            }
        }
        else
        {
            Console.WriteLine("Unknown user type.");
        }
    }

    bool IsValidEmail(string email)
    {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    Console.WriteLine("\nDone! Press any key to exit...");
    Console.ReadKey();
}

