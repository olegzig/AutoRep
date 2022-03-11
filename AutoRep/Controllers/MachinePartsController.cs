using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

using X.PagedList;

namespace AutoRep.Controllers
{
    [Authorize]
    public class MachinePartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MachinePartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MachineParts
        public async Task<IActionResult> Index(int? page, MachineParts.SortState sortOrder = Models.MachineParts.SortState.NameAsc)
        {
            //return View(await _context.MachineParts.ToListAsync());
            ViewBag.CurrentSort = sortOrder;
            IQueryable<MachineParts> parts = _context.MachineParts;

            ViewData["NameSort"] = sortOrder == Models.MachineParts.SortState.NameDesc ? Models.MachineParts.SortState.NameAsc : Models.MachineParts.SortState.NameDesc;
            ViewData["CostSort"] = sortOrder == MachineParts.SortState.CostDesc ? MachineParts.SortState.CostAsc : MachineParts.SortState.CostDesc;
            ViewData["CountSort"] = sortOrder == MachineParts.SortState.CountDesc ? MachineParts.SortState.CountAsc : MachineParts.SortState.CountDesc;

            parts = sortOrder switch
            {
                MachineParts.SortState.NameDesc => parts.OrderByDescending(x => x.Name),
                MachineParts.SortState.CostAsc => parts.OrderBy(x => x.Cost),
                MachineParts.SortState.CostDesc => parts.OrderByDescending(x => x.Cost),
                MachineParts.SortState.CountAsc => parts.OrderBy(x => x.Count),
                MachineParts.SortState.CountDesc => parts.OrderByDescending(x => x.Count),
                _ => parts.OrderBy(x => x.Name),
            };

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await parts.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: MachineParts/MachineParts/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = await _context.MachineParts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        // GET: MachineParts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MachineParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count,Discription,Cost")] MachineParts details)
        {
            if (ModelState.IsValid)
            {
                _context.Add(details);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(details);
        }

        // GET: MachineParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = await _context.MachineParts.FindAsync(id);
            if (details == null)
            {
                return NotFound();
            }
            return View(details);
        }

        // POST: MachineParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Count,Discription,Cost")] MachineParts details)
        {
            if (id != details.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(details);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailsExists(details.Id))
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
            return View(details);
        }

        // GET: MachineParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = await _context.MachineParts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        // POST: MachineParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var details = await _context.MachineParts.FindAsync(id);
            _context.MachineParts.Remove(details);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailsExists(int? id)
        {
            return _context.MachineParts.Any(e => e.Id == id);
        }
    }
}