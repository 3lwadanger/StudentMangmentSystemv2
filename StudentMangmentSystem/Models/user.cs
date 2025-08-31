using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public abstract class user
    {

            public int UserId { get; set; }
            public string Name { get; set; }
            [Required] public string Email { get; set; }
            [Required] public string Password { get; set; } 
            public char Token { get; set; }

        public abstract void showMenu();
        
    }
}
