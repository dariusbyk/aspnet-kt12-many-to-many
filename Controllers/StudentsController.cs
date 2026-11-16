using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyApp.Data;
using StudyApp.Models;

namespace StudyApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext context;

        public StudentsController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Student> items = await context.Students.Include(x => x.Courses).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            Student item = await context.Students.Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.StudentId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new StudentForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Student item = new Student();
            item.Name = model.Name;
            context.Students.Add(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Student item = await context.Students.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            StudentForm model = new StudentForm();
            model.Name = item.Name;
            ViewBag.ItemId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentForm model)
        {
            ViewBag.ItemId = id;
            Student item = await context.Students.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            item.Name = model.Name;
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Student item = await context.Students.Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.StudentId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Student item = await context.Students.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            // База автоматически удаляет зависимые записи.
            context.Students.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
