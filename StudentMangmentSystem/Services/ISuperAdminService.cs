using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Services
{
    public interface ISuperAdminService : IAdminService
    {
        void AddUser();
        void RemoveUser();

        void ViewAllAdmins();
    }

}
