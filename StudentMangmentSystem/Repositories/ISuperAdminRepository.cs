using StudentMangmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Repositories
{
    public interface ISuperAdminRepository : IAdminRepository
    {
        user GetUserByNameOrEmail(string nameOrEmail);
        Admin GetAdminByUserId(int userId);
        Student GetStudentByUserId(int userId);
        void AddAdmin(Admin admin);
        void AddStudent(Student student);
        void RemoveUser(user user);

        List<Admin> GetAllAdmins();


    }

}
