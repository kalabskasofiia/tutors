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
    public class МатеріалиController : Controller
    {
        private readonly ІстпContext _context;

        public МатеріалиController(ІстпContext context)
        {
            _context = context;
        }

        // GET: Матеріали
        public async Task<IActionResult> Index()
        {
            var істпContext = _context.Матеріалиs.Include(м => м.Lesson);
            return View(await істпContext.ToListAsync());
        }

        // GET: Матеріали/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var матеріали = await _context.Матеріалиs
                .Include(м => м.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (матеріали == null)
            {
                return NotFound();
            }

            return View(матеріали);
        }

        // GET: Матеріали/Create
        public IActionResult Create(int lessonId)
        {
            ViewBag.LessonId = lessonId;
            return View();
        }


        // POST: Матеріали/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonId,Назва")] Матеріали матеріали, IFormFile? файл)
        {
            ModelState.Remove("Lesson");
            ModelState.Remove("ФайлЗТеорією");

            if (файл != null && файл.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid() + Path.GetExtension(файл.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await файл.CopyToAsync(stream);
                матеріали.ФайлЗТеорією = "/uploads/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(матеріали);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Урок", new { id = матеріали.LessonId });
            }
            ViewBag.LessonId = матеріали.LessonId;
            return View(матеріали);
        }

        // GET: Матеріали/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var матеріали = await _context.Матеріалиs.FindAsync(id);
            if (матеріали == null)
            {
                return NotFound();
            }
            ViewData["LessonId"] = new SelectList(_context.Урокs, "Id", "Тема", матеріали.LessonId);
            return View(матеріали);
        }

        // POST: Матеріали/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonId,ФайлЗТеорією,Id")] Матеріали матеріали)
        {
            if (id != матеріали.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(матеріали);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!МатеріалиExists(матеріали.Id))
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
            ViewData["LessonId"] = new SelectList(_context.Урокs, "Id", "Тема", матеріали.LessonId);
            return View(матеріали);
        }

        // GET: Матеріали/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var матеріали = await _context.Матеріалиs
                .Include(м => м.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (матеріали == null)
            {
                return NotFound();
            }

            return View(матеріали);
        }

        // POST: Матеріали/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var матеріали = await _context.Матеріалиs.FindAsync(id);
            if (матеріали != null)
            {
                _context.Матеріалиs.Remove(матеріали);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool МатеріалиExists(int id)
        {
            return _context.Матеріалиs.Any(e => e.Id == id);
        }
    }
}
