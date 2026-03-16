using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorsDomain.Model;
using TutorsInfrastructure;

namespace TutorsInfrastructure.Controllers
{
    public class ЖурналУспішностіController : Controller
    {
        private readonly ІстпContext _context;

        public ЖурналУспішностіController(ІстпContext context)
        {
            _context = context;
        }

        // GET: ЖурналУспішності
        public async Task<IActionResult> Index()
        {
            var істпContext = _context.ЖурналУспішностіs.Include(ж => ж.Student);
            return View(await істпContext.ToListAsync());
        }

        // GET: ЖурналУспішності/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var журнал = await _context.ЖурналУспішностіs
                .Include(ж => ж.Student)
                .Include(ж => ж.Оцінкаs)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (журнал == null) return NotFound();

            return View(журнал);
        }

        // GET: ЖурналУспішності/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var журнал = await _context.ЖурналУспішностіs
                .Include(ж => ж.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (журнал == null) return NotFound();

            return View(журнал);
        }

        // POST: ЖурналУспішності/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var журнал = await _context.ЖурналУспішностіs.FindAsync(id);
            if (журнал != null)
                _context.ЖурналУспішностіs.Remove(журнал);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЖурналУспішностіExists(int id)
        {
            return _context.ЖурналУспішностіs.Any(e => e.Id == id);
        }
    }
}