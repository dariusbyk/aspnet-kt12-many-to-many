using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyApp.Data;
using StudyApp.Models;

namespace StudyApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext context;

        public TeachersController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Teacher> items = await context.Teachers.Include(x => x.Courses).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            Teacher item = await context.Teachers.Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.TeacherId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TeacherForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherForm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Teacher item = new Teacher();
            item.Name = model.Name;
            context.Teachers.Add(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Teacher item = await context.Teachers.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            TeacherForm model = new TeacherForm();
            model.Name = item.Name;
            ViewBag.ItemId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeacherForm model)
        {
            ViewBag.ItemId = id;
            Teacher item = await context.Teachers.FindAsync(id);
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
            Teacher item = await context.Teachers.Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.TeacherId == id);
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
            Teacher item = await context.Teachers.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            // База автоматически удаляет зависимые записи.
            context.Teachers.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
