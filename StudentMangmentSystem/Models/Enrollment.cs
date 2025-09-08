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
        [Required] public double Grade {  get; set; }
        [Required] public DateTime DateEnrolled { get; set; }
        public override string ToString()
        {
            return $"StudentId: {StudentId}, CourseId: {CourseId}, Grade: {Grade}, DateEnrolled: {DateEnrolled}";
        }

    }
}
