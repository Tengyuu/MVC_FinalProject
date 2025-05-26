using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using MVC_FinalProject.Data;
using MVC_FinalProject.Models;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc;
namespace MVC_FinalProject.Controllers
{
    public class StudentController : Controller
    {

        private readonly CmsContext _context;
        public StudentController(CmsContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString("Id");
            //New Add session
            if (string.IsNullOrWhiteSpace(session))
            {
                TempData["Message"] = "Please Login!";
                return RedirectToAction("Login");
            }
            ViewBag.studentid = session;
            return View();
        }
        public IActionResult Index2()
        {
            var session = HttpContext.Session.GetString("Id");
            if (string.IsNullOrWhiteSpace(session))
            {
                TempData["Message"] = "Please Login!";
                return RedirectToAction("Login");
            }
            return View();
        }
        public async Task<IActionResult> List(int? page = 1)
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
            if (page.HasValue && pageSize < 1)
            {
                return null;
            }
            var ListUnpaged = GetStufFromDatabase();
            IPagedList<Student> pagelist = ListUnpaged.ToPagedList(page ?? 1, pageSize);
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
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

            //var student = await _context.Table1121645.FirstOrDefaultAsync(m => m.Id == id);

            var student = await _context.Table1121645
                .Include(s => s.TableEnrollments1121645)
                .ThenInclude(e=>e.TableCourses1121645)
                .FirstOrDefaultAsync(m=>m.Id== id);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Gender,Password")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Table1121645.Add(student);
                await _context.SaveChangesAsync();
                TempData["CreateSuccessMessage"] = "新增成功!";
                return RedirectToAction(nameof(Index2));
            }
            ViewBag.GenderList = new List<SelectListItem>
                {
                new SelectListItem {Text = "男", Value= "男" },
                new SelectListItem {Text = "女", Value= "女" }
            };
            return View(student);
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Table1121645 == null)
            {
                return NotFound();
            }
            var student = await _context.Table1121645.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Password,Id,Name,Email,Phone,Gender")] Student student)
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index2));
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
            if (id == null || _context.Table1121645 == null)
            {
                return NotFound();
            }
            var student = await _context.Table1121645.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Table1121645 == null)
            {
                return Problem("Entity set 'CmsContext.Student' is null.");
            }
            var student = await _context.Table1121645.FindAsync(id);

            if (student != null)
            {
                _context.Table1121645.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //查詢
        [HttpGet]
        public IActionResult InputQuery()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Query()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Query(int id)
        {
            var users = await (from p in _context.Table1121645
                               where p.Id == id
                               select p).ToArrayAsync();
            return View(users);
        }

        //登入檢查
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Id, string Password)
        {
            if (Id == null && Password == null)
            {
                TempData["Message"] = "Please enter account and password!";
                return RedirectToAction("Login", "Student");
            }

            var users = await (from p in _context.Table1121645
                               where p.Id == Convert.ToInt32(Id) && p.Password == Password
                               orderby p.Name
                               select p).ToListAsync();
            if (users.Count != 0)
            {
                HttpContext.Session.SetString("Id", Id);
                HttpContext.Session.SetString("Role", users[0].Role);
                if (users[0].Role != "Admin")
                {
       

                    TempData["Loginmsg"] = "Student Logged in!";
                     return RedirectToAction("Index");
                }
                else
                {
                    TempData["LoginAdminmsg"] = "Admin Logged in!";
                    return RedirectToAction("Index2");
                }
                
            }
            else
            {
                TempData["Message"] = "Login failed!";
                return RedirectToAction("Login", "Student");
            }
        }
    

        //忘記密碼
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPassword(int id, string NewPassword)
        {
            var student = await _context.Table1121645.FindAsync(id);
            if (student == null)
            {
                ModelState.AddModelError("", "User NotFount.");
                return View();
            }
            if(string.IsNullOrWhiteSpace(NewPassword))
            {
                ModelState.AddModelError("", "Password cannot be empty.");
                return View();
            }
            student.Password = NewPassword;
            _context.Update(student);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Password reset cuccessful.";
            return RedirectToAction("Index");
        }
        //已選課程
        [HttpGet]
        public async Task<IActionResult> SelectedCourse(int? id)
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

            //var student = await _context.Table1121645.FirstOrDefaultAsync(m => m.Id == id);

            var student = await _context.Table1121645
                .Include(s => s.TableEnrollments1121645)
                .ThenInclude(e => e.TableCourses1121645)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

    }
}
