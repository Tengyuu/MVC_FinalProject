using System.ComponentModel.DataAnnotations;

namespace MVC_FinalProject.Models.ViewModels
{
    public class CourseWithEnrollCountViewModel
    {
        
        public int CourseId { get; set; }
        [Display(Name =("課程名稱"))]
        public string CourseName { get; set; }
        [Display(Name = ("老師"))]
        public string Teacher { get; set; }
        [Display(Name = ("名額"))]
        public int MaxCapacity { get; set; }
        [Display(Name = ("已選人數"))]
        public int EnrolledCount { get; set; }
    }
}
