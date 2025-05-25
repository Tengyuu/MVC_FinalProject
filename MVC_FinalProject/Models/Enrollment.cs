using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_FinalProject.Models
{
    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentId {  get; set; }
        public int StudentId { get; set; }
        public Student Table1121645 { get; set; }
        public int CourseId {  get; set; }
        public Course TableCourses1121645 { get; set; }
       
    }
}
