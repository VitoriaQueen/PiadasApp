using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PiadasApp.Data;
using PiadasApp.Models;

namespace PiadasApp.Controllers
{
    public class PiadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PiadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Piadas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Piada.ToListAsync());
        }
        
        // GET: Piadas/MostreFormularioDeProcura
        public async Task<IActionResult> MostreFormularioDeBusca()
        {
            return View();
        }
        // Post: Piadas/MostreResultadosDabusca
        public async Task<IActionResult> MostreResultadosDaBusca(String ProcureFrase)
        {
            return View("Index", await _context.Piada.Where( j => j.PiadaPergunta.Contains(ProcureFrase)).ToListAsync());
        }

        // GET: Piadas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piada = await _context.Piada
                .FirstOrDefaultAsync(m => m.Id == id);
            if (piada == null)
            {
                return NotFound();
            }

            return View(piada);
        }

        // GET: Piadas/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Piadas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PiadaPergunta,PiadaResposta")] Piada piada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(piada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(piada);
        }

        // GET: Piadas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piada = await _context.Piada.FindAsync(id);
            if (piada == null)
            {
                return NotFound();
            }
            return View(piada);
        }

        // POST: Piadas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PiadaPergunta,PiadaResposta")] Piada piada)
        {
            if (id != piada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(piada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PiadaExists(piada.Id))
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
            return View(piada);
        }

        // GET: Piadas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var piada = await _context.Piada
                .FirstOrDefaultAsync(m => m.Id == id);
            if (piada == null)
            {
                return NotFound();
            }

            return View(piada);
        }

        // POST: Piadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var piada = await _context.Piada.FindAsync(id);
            _context.Piada.Remove(piada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PiadaExists(int id)
        {
            return _context.Piada.Any(e => e.Id == id);
        }
    }
}
