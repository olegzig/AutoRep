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
    public class WorkTypeC : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkTypeC(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkTypeC
        public async Task<IActionResult> Index(int? page, WorkType.SortState sortOrder = WorkType.SortState.NameAsc)
        {
            ViewBag.CurrentSort = sortOrder;
            IQueryable<WorkType> worksTypes = _context.WorkType;

            ViewData["NameSort"] = sortOrder == WorkType.SortState.NameDesc ? WorkType.SortState.NameAsc : WorkType.SortState.NameDesc;
            ViewData["CostSort"] = sortOrder == WorkType.SortState.CostDesc ? WorkType.SortState.CostAsc : WorkType.SortState.CostDesc;

            worksTypes = sortOrder switch
            {
                WorkType.SortState.NameDesc => worksTypes.OrderByDescending(x => x.Name),
                WorkType.SortState.CostAsc => worksTypes.OrderBy(x => x.Cost),
                WorkType.SortState.CostDesc => worksTypes.OrderByDescending(x => x.Cost),
                _ => worksTypes.OrderBy(x => x.Name),
            };
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(await worksTypes.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: WorkTypeC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workType = await _context.WorkType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workType == null)
            {
                return NotFound();
            }

            return View(workType);
        }

        // GET: WorkTypeC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkTypeC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Text,Cost")] WorkType workType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workType);
        }

        // GET: WorkTypeC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workType = await _context.WorkType.FindAsync(id);
            if (workType == null)
            {
                return NotFound();
            }
            return View(workType);
        }

        // POST: WorkTypeC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Text,Cost")] WorkType workType)
        {
            if (id != workType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkTypeExists(workType.Id))
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
            return View(workType);
        }

        // GET: WorkTypeC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workType = await _context.WorkType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workType == null)
            {
                return NotFound();
            }

            return View(workType);
        }

        // POST: WorkTypeC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workType = await _context.WorkType.FindAsync(id);
            _context.WorkType.Remove(workType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkTypeExists(int? id)
        {
            return _context.WorkType.Any(e => e.Id == id);
        }
    }
}