using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyApp.Data;
using StudyApp.Models;

namespace StudyApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext context;

        public CoursesController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Course> items = await context.Courses.Include(x => x.Teacher).Include(x => x.Students).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            Course item = await context.Courses.Include(x => x.Teacher).Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.CourseId == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadTeachers();
            return View(new CourseForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseForm model)
        {
            if (!await context.Teachers.AnyAsync(a => a.TeacherId == model.TeacherId))
            {
                ModelState.AddModelError("TeacherId", "Выберите существующего преподавателя.");
            }
            if (!ModelState.IsValid)
            {
                await LoadTeachers(model.TeacherId);
                return View(model);
            }
            Course item = new Course();
            item.Title = model.Title;
            item.TeacherId = model.TeacherId;
            item.Students.Clear();
            List<Student> selectedStudents = await context.Students
                .Where(s => model.StudentIds.Contains(s.StudentId)).ToListAsync();
            foreach (Student student in selectedStudents)
            {
                item.Students.Add(student);
            }
            context.Courses.Add(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Course item = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.CourseId == id);
            if (item == null)
            {
                return NotFound();
            }
            CourseForm model = new CourseForm();
            model.Title = item.Title;
            model.TeacherId = item.TeacherId;
            foreach (Student student in item.Students)
            {
                model.StudentIds.Add(student.StudentId);
            }
            await LoadTeachers(item.TeacherId);
            ViewBag.ItemId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseForm model)
        {
            ViewBag.ItemId = id;
            Course item = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.CourseId == id);
            if (item == null)
            {
                return NotFound();
            }
            if (!await context.Teachers.AnyAsync(a => a.TeacherId == model.TeacherId))
            {
                ModelState.AddModelError("TeacherId", "Выберите существующего преподавателя.");
            }
            if (!ModelState.IsValid)
            {
                await LoadTeachers(model.TeacherId);
                return View(model);
            }
            item.Title = model.Title;
            item.TeacherId = model.TeacherId;
            item.Students.Clear();
            List<Student> selectedStudents = await context.Students
                .Where(s => model.StudentIds.Contains(s.StudentId)).ToListAsync();
            foreach (Student student in selectedStudents)
            {
                item.Students.Add(student);
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Course item = await context.Courses.Include(x => x.Teacher).Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.CourseId == id);
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
            Course item = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.CourseId == id);
            if (item == null)
            {
                return NotFound();
            }
            context.Courses.Remove(item);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task LoadTeachers(int selectedId = 0)
        {
            List<Teacher> teachers = await context.Teachers.OrderBy(a => a.Name).ToListAsync();
            ViewBag.Teachers = new SelectList(teachers, "TeacherId", "Name", selectedId);
            ViewBag.HasTeachers = teachers.Count > 0;
            ViewBag.Students = await context.Students.OrderBy(s => s.Name).ToListAsync();
        }
    }
}
