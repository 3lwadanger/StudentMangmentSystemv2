using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public abstract class user
    {

            [Key] public int UserId { get; set; }
            [Required] public string Name { get; set; }
            [EmailAddress] public string Email { get; set; }
            [PasswordPropertyText] public string Password { get; set; } 
            [Required]public char Token { get; set; }

        public abstract void showMenu();
        
    }
}
