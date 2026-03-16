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
    public class ОцінкаController : Controller
    {
        private readonly ІстпContext _context;

        public ОцінкаController(ІстпContext context)
        {
            _context = context;
        }

        // GET: Оцінка
        public async Task<IActionResult> Index()
        {
            var істпContext = _context.Оцінкаs.Include(о => о.Assigment).Include(о => о.Gradebook);
            return View(await істпContext.ToListAsync());
        }

        // GET: Оцінка/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var оцінка = await _context.Оцінкаs
                .Include(о => о.Assigment)
                .Include(о => о.Gradebook)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (оцінка == null)
            {
                return NotFound();
            }

            return View(оцінка);
        }

        // GET: Оцінка/Create
        public IActionResult Create(int gradebookId)
        {
            var журнал = _context.ЖурналУспішностіs
                .Include(ж => ж.Student)
                .ThenInclude(u => u.Урокs)
                .ThenInclude(у => у.Завданняs)
                .FirstOrDefault(ж => ж.Id == gradebookId);

            var завдання = журнал?.Student.Урокs
                .SelectMany(у => у.Завданняs)
                .ToList();

            ViewBag.GradebookId = gradebookId;
            ViewData["AssigmentId"] = new SelectList(завдання, "Id", "ОписЗавдання");
            return View();
        }
        // POST: Оцінка/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssigmentId,GradebookId,Бал,МаксимальнийБал")] Оцінка оцінка)
        {
            ModelState.Remove("Assigment");
            ModelState.Remove("Gradebook");

            if (ModelState.IsValid)
            {
                _context.Add(оцінка);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ЖурналУспішності", new { id = оцінка.GradebookId });
            }

            ViewBag.GradebookId = оцінка.GradebookId;
            ViewData["AssigmentId"] = new SelectList(_context.Завданняs, "Id", "ОписЗавдання", оцінка.AssigmentId);
            return View(оцінка);
        }

        // GET: Оцінка/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var оцінка = await _context.Оцінкаs.FindAsync(id);
            if (оцінка == null)
            {
                return NotFound();
            }
            ViewData["AssigmentId"] = new SelectList(_context.Завданняs, "Id", "Id", оцінка.AssigmentId);
            ViewData["GradebookId"] = new SelectList(_context.ЖурналУспішностіs, "Id", "Id", оцінка.GradebookId);
            return View(оцінка);
        }

        // POST: Оцінка/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssigmentId,GradebookId,Бал,МаксимальнийБал,Id")] Оцінка оцінка)
        {
            if (id != оцінка.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(оцінка);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ОцінкаExists(оцінка.Id))
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
            ViewData["AssigmentId"] = new SelectList(_context.Завданняs, "Id", "Id", оцінка.AssigmentId);
            ViewData["GradebookId"] = new SelectList(_context.ЖурналУспішностіs, "Id", "Id", оцінка.GradebookId);
            return View(оцінка);
        }

        // GET: Оцінка/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var оцінка = await _context.Оцінкаs
                .Include(о => о.Assigment)
                .Include(о => о.Gradebook)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (оцінка == null)
            {
                return NotFound();
            }

            return View(оцінка);
        }

        // POST: Оцінка/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var оцінка = await _context.Оцінкаs.FindAsync(id);
            if (оцінка != null)
            {
                _context.Оцінкаs.Remove(оцінка);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ОцінкаExists(int id)
        {
            return _context.Оцінкаs.Any(e => e.Id == id);
        }
    }
}
