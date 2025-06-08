using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_FinalProject.Models
{
    [Table("TableStudents1121645")]
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="請輸入姓名!")]
        [Display(Name="姓名")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string Gender { get; set; }

        public string Password { get; set; }
        public string Role { get; set; } 

        public ICollection<Enrollment> TableEnrollments1121645 { get; set; } = new List<Enrollment>();
        
    }
}
