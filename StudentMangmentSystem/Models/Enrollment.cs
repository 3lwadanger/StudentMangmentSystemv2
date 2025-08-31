using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public class Enrollment
    {
        
        [Key] public int StudentId    { get; set; }
        [Key] public int CourseId {  get; set; }
        public double Grade {  get; set; }
        public DateTime DateEnrolled { get; set; }

    }
}
