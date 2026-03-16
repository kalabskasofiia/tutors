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
    public class ВчительController : Controller
    {
        private readonly ІстпContext _context;

        public ВчительController(ІстпContext context)
        {
            _context = context;
        }

        // GET: Вчитель
        public async Task<IActionResult> Index()
        {
            return View(await _context.Вчительs.ToListAsync());
        }

        // GET: Вчитель/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var вчитель = await _context.Вчительs
                .Include(v => v.Ученьs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (вчитель == null) return NotFound();

            return View(вчитель);
        }

        // GET: Вчитель/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Вчитель/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Піб,Профіль")] Вчитель вчитель)
        {
            ModelState.Remove("Урокs");
            ModelState.Remove("Ученьs");

            if (ModelState.IsValid)
            {
                _context.Add(вчитель);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(вчитель);
        }

        // GET: Вчитель/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вчитель = await _context.Вчительs.FindAsync(id);
            if (вчитель == null)
            {
                return NotFound();
            }
            return View(вчитель);
        }

        // POST: Вчитель/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,Піб,Профіль,Id")] Вчитель вчитель)
        {
            if (id != вчитель.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(вчитель);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВчительExists(вчитель.Id))
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
            return View(вчитель);
        }

        // GET: Вчитель/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var вчитель = await _context.Вчительs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (вчитель == null)
            {
                return NotFound();
            }

            return View(вчитель);
        }

        // POST: Вчитель/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var вчитель = await _context.Вчительs.FindAsync(id);
            if (вчитель != null)
            {
                _context.Вчительs.Remove(вчитель);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВчительExists(int id)
        {
            return _context.Вчительs.Any(e => e.Id == id);
        }
    }
}
