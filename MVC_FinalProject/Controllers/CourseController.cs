﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_FinalProject.Data;
using MVC_FinalProject.Models;
using MVC_FinalProject.Models.ViewModels;

namespace MVC_FinalProject.Controllers
{
    public class CourseController : Controller
    {
        public readonly CmsContext _context;
        public CourseController(CmsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _context.TableCourses1121645.ToListAsync();
            var viewmodel = await _context.TableCourses1121645.Select(course => new CourseWithEnrollCountViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Teacher = course.Teacher,
                MaxCapacity = course.MaxCapacity,
                EnrolledCount = _context.TableEnrollments1121645.Count(e=>e.CourseId == course.CourseId)
            })
            .ToListAsync();

            return View(viewmodel);
        }
        //create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,Teacher,MaxCapacity")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.TableCourses1121645.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        //edit
        public async Task<IActionResult> Edit(int? id)//id可為null
        {
            if(id == null || _context.TableCourses1121645 == null)
            {
                return NotFound();
            }
            var course = await _context.TableCourses1121645.FindAsync(id);
            if(course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,Teacher,MaxCapacity")] Course course )
        {
            if(id!=course.CourseId)
            {
                return NotFound();
            }
            var courseExit = await _context.TableCourses1121645.AnyAsync(c=>c.CourseId != course.CourseId && c.CourseName == course.CourseName && c.Teacher == course.Teacher);
            if (courseExit)                                     //從資料庫中這一筆資料的課程名稱 = 使用者輸入送過來的課程名稱。
            {
                                        //("CourseName", "已存在相同資料!")錯誤訊息加在CourseName欄位下
                ModelState.AddModelError("CourseName", "已存在相同資料!");
                return View(course);
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _context.TableCourses1121645.Update(course);
                    await _context.SaveChangesAsync();
                    
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Editmsg"] = "編輯成功";
                return RedirectToAction("Index");
            }
            return View(course);
        }
        private bool CourseExists(int id)
        {
            return _context.TableCourses1121645.Any(e=>e.CourseId==id);
        }
        //delete
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var course = await _context.TableCourses1121645.FirstOrDefaultAsync(c => c.CourseId == id);
            if(course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteComfirmed(int id)
        {
            if(_context.TableCourses1121645 == null)
            {
                return Problem("Entity set 'CmsContext.Course' is null.");
            }
            var course = await _context.TableCourses1121645.FindAsync(id);
            if(course!=null)
            {
                _context.TableCourses1121645.Remove(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        //Detail
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TableCourses1121645 == null)
            {
                var msgObject = new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    error = "無效請求，必須提供id!"
                };
                return new BadRequestObjectResult(msgObject);
            }
            var course = await _context.TableCourses1121645.FirstOrDefaultAsync(c=>c.CourseId == id);
            if(course==null)
            {
                return NotFound();
            }
            return View(course);
        }
    }
}
