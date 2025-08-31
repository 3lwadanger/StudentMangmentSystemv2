using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangmentSystem.Models
{
    public class Course
    {
        [Key] public int CourseId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Credits {  get; set; }

        public override string ToString()
        {
            return $"Course Name: {Title}, Code: {Code}, Credits {Credits}";
        }

    }
}
