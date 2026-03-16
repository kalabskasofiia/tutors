using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorsDomain.Model;
using TutorsInfrastructure;

namespace TutorsInfrastructure.Controllers
{
    public class УченьController : Controller
    {
        private readonly ІстпContext _context;

        public УченьController(ІстпContext context)
        {
            _context = context;
        }

        // GET: Учень
        public async Task<IActionResult> Index()
        {
            var істпContext = _context.Ученьs.Include(у => у.Teacher);
            return View(await істпContext.ToListAsync());
        }

        // GET: Учень/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var учень = await _context.Ученьs
                .Include(u => u.Урокs)
                .Include(u => u.Teacher)
                .Include(u => u.ЖурналУспішності)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (учень == null) return NotFound();

            // оновлюємо статуси уроків автоматично
            var сьогодні = DateOnly.FromDateTime(DateTime.Today);
            bool змінено = false;
            foreach (var урок in учень.Урокs)
            {
                var новийСтатус = урок.Дата < сьогодні ? "Проведено" : "Заплановано";
                if (урок.СтатусУроку != новийСтатус)
                {
                    урок.СтатусУроку = новийСтатус;
                    змінено = true;
                }
            }
            if (змінено)
                await _context.SaveChangesAsync();

            return View(учень);
        }
        // GET: Учень/Create
        public IActionResult Create(int teacherId)
        {
            ViewBag.TeacherId = teacherId;
            ViewData["TeacherId"] = new SelectList(_context.Вчительs, "Id", "Email");
            return View();
        }

        // POST: Учень/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Учень/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,Email,Піб,Клас")] Учень учень)
        {
            ModelState.Remove("Teacher");
            ModelState.Remove("ЖурналУспішності");
            ModelState.Remove("Урокs");

            if (ModelState.IsValid)
            {
                _context.Add(учень);
                await _context.SaveChangesAsync();

                // автоматично створюємо журнал
                var журнал = new ЖурналУспішності
                {
                    StudentId = учень.Id
                };
                _context.Add(журнал);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Вчитель", new { id = учень.TeacherId });
            }

            ViewData["TeacherId"] = new SelectList(_context.Вчительs, "Id", "Email", учень.TeacherId);
            return View(учень);
        }

        // GET: Учень/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var учень = await _context.Ученьs.FindAsync(id);
            if (учень == null)
            {
                return NotFound();
            }
            ViewData["TeacherId"] = new SelectList(_context.Вчительs, "Id", "Email", учень.TeacherId);
            return View(учень);
        }

        // POST: Учень/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,Email,Піб,Клас,Id")] Учень учень)
        {
            if (id != учень.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(учень);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!УченьExists(учень.Id))
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
            ViewData["TeacherId"] = new SelectList(_context.Вчительs, "Id", "Email", учень.TeacherId);
            return View(учень);
        }

        // GET: Учень/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var учень = await _context.Ученьs
                .Include(у => у.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (учень == null)
            {
                return NotFound();
            }

            return View(учень);
        }

        // POST: Учень/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var учень = await _context.Ученьs.FindAsync(id);
            if (учень != null)
            {
                _context.Ученьs.Remove(учень);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool УченьExists(int id)
        {
            return _context.Ученьs.Any(e => e.Id == id);
        }
    }
}
