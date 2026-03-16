using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorsDomain.Model;
using TutorsInfrastructure;

namespace TutorsInfrastructure.Controllers
{
    public class ЗавданняController : Controller
    {
        private readonly ІстпContext _context;

        public ЗавданняController(ІстпContext context)
        {
            _context = context;
        }

        // GET: Завдання
        public async Task<IActionResult> Index()
        {
            var істпContext = _context.Завданняs.Include(з => з.Lesson);
            return View(await істпContext.ToListAsync());
        }

        // GET: Завдання/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var завдання = await _context.Завданняs
                .Include(з => з.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (завдання == null)
            {
                return NotFound();
            }

            return View(завдання);
        }

        // GET: Завдання/Create
        public IActionResult Create(int lessonId)
        {
            ViewBag.LessonId = lessonId;
            return View();
        }

        // POST: Завдання/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonId,ОписЗавдання,Дедлайн,ДатаЗдачі,ТипЗавдання")] Завдання завдання, IFormFile? файл)
        {
            ModelState.Remove("Lesson");
            ModelState.Remove("ЖурналУспішностіs");
            ModelState.Remove("Оцінкаs");

            if (файл != null && файл.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(файл.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await файл.CopyToAsync(stream);
                завдання.ФайлШлях = "/uploads/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(завдання);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Урок", new { id = завдання.LessonId });
            }
            ViewBag.LessonId = завдання.LessonId;
            return View(завдання);
        }
        // GET: Завдання/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var завдання = await _context.Завданняs.FindAsync(id);
            if (завдання == null)
            {
                return NotFound();
            }
            ViewData["LessonId"] = new SelectList(_context.Урокs, "Id", "Тема", завдання.LessonId);
            return View(завдання);
        }

        // POST: Завдання/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonId,ОписЗавдання,Дедлайн,ДатаЗдачі,ТипЗавдання,Id")] Завдання завдання)
        {
            if (id != завдання.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(завдання);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ЗавданняExists(завдання.Id))
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
            ViewData["LessonId"] = new SelectList(_context.Урокs, "Id", "Тема", завдання.LessonId);
            return View(завдання);
        }

        // GET: Завдання/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var завдання = await _context.Завданняs
                .Include(з => з.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (завдання == null)
            {
                return NotFound();
            }

            return View(завдання);
        }

        // POST: Завдання/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var завдання = await _context.Завданняs.FindAsync(id);
            if (завдання != null)
            {
                _context.Завданняs.Remove(завдання);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЗавданняExists(int id)
        {
            return _context.Завданняs.Any(e => e.Id == id);
        }
        // GET: Завдання/ЗдатиРоботу/5
        public async Task<IActionResult> ЗдатиРоботу(int? id)
        {
            if (id == null) return NotFound();
            var завдання = await _context.Завданняs.FindAsync(id);
            if (завдання == null) return NotFound();
            return View("SubmitWork", завдання);
        }

        // POST: Завдання/ЗдатиРоботу/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ЗдатиРоботу(int id, IFormFile? файл)
        {
            var завдання = await _context.Завданняs.FindAsync(id);
            if (завдання == null) return NotFound();

            if (файл != null && файл.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(файл.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await файл.CopyToAsync(stream);
                завдання.ФайлУчня = "/uploads/" + fileName;
                завдання.ДатаЗдачі = DateOnly.FromDateTime(DateTime.Today);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = завдання.Id });
        }
    }
}
