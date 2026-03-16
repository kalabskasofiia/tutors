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
    public class УрокController : Controller
    {
        private readonly ІстпContext _context;

        public УрокController(ІстпContext context)
        {
            _context = context;
        }

        // GET: Урок
        public async Task<IActionResult> Index()
        {
            var істпContext = _context.Урокs.Include(у => у.Student).Include(у => у.Teacher);
            return View(await істпContext.ToListAsync());
        }

        // GET: Урок/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var урок = await _context.Урокs
                .Include(у => у.Student)
                .Include(у => у.Teacher)
                .Include(у => у.Завданняs)   
                .Include(у => у.Матеріалиs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (урок == null)
            {
                return NotFound();
            }

            return View(урок);
        }

        // GET: Урок/Create
        public IActionResult Create(int studentId, int teacherId)
        {
            ViewBag.StudentId = studentId;
            ViewBag.TeacherId = teacherId;
            return View();
        }

        // POST: Урок/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,TeacherId,Тема,Дата,ТривалістьУроку")] Урок урок)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Teacher");

            // встановлюємо статус автоматично
            var сьогодні = DateOnly.FromDateTime(DateTime.Today);
            урок.СтатусУроку = урок.Дата < сьогодні ? "Проведено" : "Заплановано";

            if (ModelState.IsValid)
            {
                _context.Add(урок);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Учень", new { id = урок.StudentId });
            }
            ViewBag.StudentId = урок.StudentId;
            ViewBag.TeacherId = урок.TeacherId;
            return View(урок);
        }

        // GET: Урок/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var урок = await _context.Урокs.FindAsync(id);
            if (урок == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Ученьs, "Id", "Email", урок.StudentId);
            ViewData["TeacherId"] = new SelectList(_context.Вчительs, "Id", "Email", урок.TeacherId);
            return View(урок);
        }

        // POST: Урок/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,TeacherId,Тема,Дата,СтатусУроку,ТривалістьУроку,Id")] Урок урок)
        {
            if (id != урок.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(урок);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!УрокExists(урок.Id))
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
            ViewData["StudentId"] = new SelectList(_context.Ученьs, "Id", "Email", урок.StudentId);
            ViewData["TeacherId"] = new SelectList(_context.Вчительs, "Id", "Email", урок.TeacherId);
            return View(урок);
        }

        // GET: Урок/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var урок = await _context.Урокs
                .Include(у => у.Student)
                .Include(у => у.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (урок == null)
            {
                return NotFound();
            }

            return View(урок);
        }

        // POST: Урок/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var урок = await _context.Урокs.FindAsync(id);
            if (урок != null)
            {
                _context.Урокs.Remove(урок);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool УрокExists(int id)
        {
            return _context.Урокs.Any(e => e.Id == id);
        }
    }
}