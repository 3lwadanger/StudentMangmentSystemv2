using Microsoft.EntityFrameworkCore;
using StudentMangmentSystem.Data;
using StudentMangmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Repositories
{
    public class SuperAdminRepository : AdminRepository, ISuperAdminRepository
    {
        private readonly StudentMangmentSystemDbContext _context;

        public SuperAdminRepository(StudentMangmentSystemDbContext context) : base(context)
        {
            _context = context;
        }

        public user GetUserByNameOrEmail(string nameOrEmail) =>
            _context.Users.FirstOrDefault(u =>
                EF.Functions.Collate(u.Email, "SQL_Latin1_General_CP1_CS_AS") == nameOrEmail ||
                EF.Functions.Collate(u.Name, "SQL_Latin1_General_CP1_CS_AS") == nameOrEmail);

        public Admin GetAdminByUserId(int userId) =>
            _context.Admins.FirstOrDefault(a => a.UserId == userId);

        public Student GetStudentByUserId(int userId) =>
            _context.Students.FirstOrDefault(s => s.UserId == userId);

        public void AddAdmin(Admin admin) => _context.Admins.Add(admin);
        public void AddStudent(Student student) => _context.Students.Add(student);

        public void RemoveUser(user user) => _context.Users.Remove(user);

        public List<Admin> GetAllAdmins() => _context.Admins.AsNoTracking().ToList();

    }


}
