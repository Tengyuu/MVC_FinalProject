using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_FinalProject.Data;
using MVC_FinalProject.Models;

namespace MVC_FinalProject.Controllers
{
    public class EnrollmentController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly CmsContext _context;
        public EnrollmentController(CmsContext context) 
        {
            _context = context;
        }
        //顯示所有課程供學生選擇
        public async Task<IActionResult> Enroll()
        {
            var sessionId = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(sessionId))
            {
                TempData["Message"] = "Please Login!";
                return RedirectToAction("Login", "Student");
            }
            int studentId = int.Parse(sessionId);
            var courses = await _context.TableCourses1121645.ToListAsync();
            ViewBag.StudentId = studentId;
            return View(courses);
        }
        //執行選課
        [HttpPost]
        public async Task<IActionResult> EnrollCourse(int courseId)
        {
            var sessionId = HttpContext.Session.GetString("Id");
            if (string.IsNullOrEmpty(sessionId))
            {
                TempData["Message"] = "Please Login!";
                return RedirectToAction("Login", "Student");
            }
            int studentId = int.Parse(sessionId);
          
            var course = await _context.TableCourses1121645
                .Include(c=>c.TableEnrollments1121645)
                .FirstOrDefaultAsync(c=> c.CourseId == courseId);

            bool alreadyEnrolled = course.TableEnrollments1121645.Any(e => e.StudentId == studentId && e.CourseId == courseId);
            int currentCapacity = course.TableEnrollments1121645.Count();

     

            if (alreadyEnrolled)
            {
                TempData["Enrollmsg"] = $"{course.CourseName} 已經選過了！";
            }
            else if(currentCapacity >= course.MaxCapacity)
            {
                TempData["Enrollmsg"] = $"{course.CourseName} 人數已滿，選課失敗！";
            }
            else
            {
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId
                };
                _context.TableEnrollments1121645.Add(enrollment);
                await _context.SaveChangesAsync();
                TempData["Enrollmsg"] = $"{course.CourseName}選課成功!";
            }
           
            return RedirectToAction("Enroll");
            //return RedirectToAction("Details","Student",new {id = studentId});
        }
        //退選課程
        public async Task<IActionResult> DropCourse(int studentId, int courseId)
        {
            var enrollment = await _context.TableEnrollments1121645.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
            var course = await _context.TableCourses1121645.FindAsync(courseId);
            if (enrollment != null)
            {
                _context.TableEnrollments1121645.Remove(enrollment);
                await _context.SaveChangesAsync();
                TempData["DropSuccessmsg"] = $"{course.CourseName} 退選成功!";
            }
            return RedirectToAction("SelectedCourse", "Student", new { id = studentId });
        }
        
    }
}
