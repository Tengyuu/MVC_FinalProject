using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_FinalProject.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        [Display(Name = "課程名稱")]
        public string CourseName { get; set; }
        [Display(Name = "老師")]
        public string Teacher {  get; set; }
        public ICollection<Enrollment> TableEnrollments1121645 { get; set; } = new List<Enrollment>();
     
    }
}
