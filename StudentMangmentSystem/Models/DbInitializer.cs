using System;
using System.Linq;
using System.Collections.Generic;

namespace StudentMangmentSystem.Models
{
    public static class DbInitializer
    {
        public static void Seed(StudentMangmentSystemDbContext context)
        {
            context.Database.EnsureCreated();

            // If there are already users, return
            if (context.Users.Any())
            {
                return; 
            }

            // ================== Admin ==================
            var admin = new Admin
            {
                Name = "System Admin",
                Email = "admin@system.com",
                Password = "admin123",
                Token = 'A'
            };

            // ================== Students ==================
            var students = new Student[]
            {
                new Student { Name = "Ali Hassan", Email = "ali@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Sara Ahmed", Email = "sara@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Omar Khaled", Email = "omar@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Mona Youssef", Email = "mona@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Khaled Fathy", Email = "khaled@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Fatma Ibrahim", Email = "fatma@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Mohamed Adel", Email = "mohamed@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Layla Hussein", Email = "layla@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Youssef Samir", Email = "youssef@student.com", Password = "1234", Token = 'S' },
                new Student { Name = "Nourhan Ali", Email = "nourhan@student.com", Password = "1234", Token = 'S' }
            };

            // ================== Courses (with Credits) ==================
            var courses = new Course[]
            {
                new Course { Title = "Mathematics",         Code = "MATH101", Credits = 3 },
                new Course { Title = "Computer Science",    Code = "CS102",   Credits = 4 },
                new Course { Title = "History",             Code = "HIS103",  Credits = 2 },
                new Course { Title = "Physics",             Code = "PHY104",  Credits = 4 },
                new Course { Title = "English Literature",  Code = "ENG105",  Credits = 3 },
                new Course { Title = "Economics",           Code = "ECO106",  Credits = 3 },
                new Course { Title = "Biology",             Code = "BIO107",  Credits = 4 },
                new Course { Title = "Philosophy",          Code = "PHI108",  Credits = 2 }
            };

            // ================== Add & Save ==================
            context.Users.Add(admin);
            context.Users.AddRange(students);
            context.Courses.AddRange(courses);
            context.SaveChanges(); // save so UserId and CourseId are generated

            // ================== Enrollments (credit-based) ==================
            var random = new Random();
            var enrollments = new List<Enrollment>();

            // For each student pick random courses until the student's credit load >= target credits
            foreach (var student in students)
            {
                int targetCredits = random.Next(9, 19); // target between 9 and 18 credits
                int accumulated = 0;

                // available courses list (shuffled)
                var availableCourses = courses.OrderBy(c => random.Next()).ToList();
                int idx = 0;

                while (accumulated < targetCredits && idx < availableCourses.Count)
                {
                    var course = availableCourses[idx++];
                    // avoid duplicate enrollment for the same course
                    if (enrollments.Any(e => e.StudentId == student.UserId && e.CourseId == course.CourseId))
                        continue;

                    // add enrollment with grade out of 100
                    enrollments.Add(new Enrollment
                    {
                        StudentId = student.UserId,
                        CourseId = course.CourseId,
                        Grade = random.Next(60, 101), // grade 60..100
                        DateEnrolled = DateTime.Now.AddDays(-random.Next(1, 400))
                    });

                    accumulated += course.Credits;
                }

                // If no courses were added (edge case), ensure at least one enrollment
                if (!enrollments.Any(e => e.StudentId == student.UserId))
                {
                    var fallback = courses[random.Next(courses.Length)];
                    enrollments.Add(new Enrollment
                    {
                        StudentId = student.UserId,
                        CourseId = fallback.CourseId,
                        Grade = random.Next(60, 101),
                        DateEnrolled = DateTime.Now.AddDays(-random.Next(1, 400))
                    });
                }
            }

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}





