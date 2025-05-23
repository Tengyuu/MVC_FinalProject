namespace MVC_FinalProject.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Teacher {  get; set; }
        public ICollection<Enrollment> TableEnrollments1121645 { get; set; }
    }
}
