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
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Table1121645, "Id", "Name");
            ViewBag.Courses = new SelectList(_context.TableCourses1121645, "CourseId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,CourseId")] Enrollment enrollment)
        {
            // 檢查是否已經選過該課
            bool alreadyEnrolled = _context.TableEnrollments1121645
                .Any(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId);

            if (alreadyEnrolled)
            {
                ModelState.AddModelError("", "學生已經選過這門課。");
            }

            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Students = new SelectList(_context.Table1121645, "Id", "Name", enrollment.StudentId);
            ViewBag.Courses = new SelectList(_context.TableCourses1121645, "CourseId", "Title", enrollment.CourseId);
            return View(enrollment);
        }

    }
}
