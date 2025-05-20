using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models
{
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
        public DateTime CreateDate { get; set; }
        public string Gender { get; set; }
    }
}
