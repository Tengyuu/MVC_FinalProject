using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_FinalProject.Data;
using MVC_FinalProject.Models;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc;
namespace MVC_FinalProject.Controllers
{
    public class DBStudentController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly CmsContext _context;
        public DBStudentController(CmsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page = 1)
        {
            //分頁
            const int pageSize = 5;
            ViewBag.userModel = GetPagedProcess(page, pageSize);
            //var users = await _context.Table1121645.ToListAsync(); 
            var users = await _context.Table1121645
                .OrderBy(s => s.Id)
                .Skip(pageSize * ((page ?? 1) - 1))
                .Take(pageSize)
                .ToArrayAsync();
            return View(users);
        }
        protected IPagedList<Student> GetPagedProcess(int? page, int pageSize)
        {
            if(page.HasValue && pageSize < 1)
            {
                return null;
            }
            var ListUnpaged = GetStufFromDatabase();
            IPagedList<Student> pagelist = ListUnpaged.ToPagedList(page ?? 1 , pageSize);
            if(pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
            {
                return null;
            }
            return pagelist;
        }
        protected IQueryable<Student> GetStufFromDatabase()
        {
            return _context.Table1121645;
        }
        //Detail
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Table1121645 == null)
            {
                var msgObject = new
                {
                    statuscode = StatusCodes.Status400BadRequest,
                    error = "無效的請求，必須提供Id編號!"
                };
                return new BadRequestObjectResult(msgObject);
            }

            var student = await _context.Table1121645.FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        //Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.GenderList = new List<SelectListItem>
            {
                new SelectListItem {Text = "男", Value= "男" },
                new SelectListItem {Text = "女", Value= "女" }
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,CreateDate,Gender")]Student student)
        {
            if(ModelState.IsValid)
            {
                _context.Table1121645.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if( id == null || _context.Table1121645 == null)
            {
                return NotFound();
            }
            var student = await _context.Table1121645.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id,Name,Email,Phone,CreateDate,Gender")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Table1121645.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }             
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        private bool StudentExists(int id)
        {
            return _context.Table1121645.Any(m => m.Id == id);
        }

        //Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || _context.Table1121645 == null)
            {
                return NotFound();
            }
            var student = await _context.Table1121645.FirstOrDefaultAsync(m => m.Id == id);
            if(student == null) 
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_context.Table1121645 == null)
            {
                return Problem("Entity set 'CmsContext.Student' is null.");
            }
            var student = await _context.Table1121645.FindAsync(id);

            if(student != null)
            {
                _context.Table1121645.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
