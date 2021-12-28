using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Controllers
{
    public class UserC : Controller
    {
        private readonly AuthContext _context;
        public UserC(AuthContext context)
        {
            _context = context;
        }

        // GET: UserC
        [Authorize]
        public async Task<IActionResult> Index(SUser.SortState sortOrder = Models.SUser.SortState.NameAsc)
        {
            IQueryable<SUser> users = _context.Users;

            ViewData["NameSort"] = sortOrder == Models.SUser.SortState.NameDesc ? Models.SUser.SortState.NameAsc : Models.SUser.SortState.NameDesc;

            users = sortOrder switch
            {
                Models.SUser.SortState.NameDesc => users.OrderByDescending(x => x.UserName),
                _ => users.OrderBy(x => x.UserName),
            };
            return View(await users.AsNoTracking().ToListAsync());
        }

        // GET: UserC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UserClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UserC/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsOwner")] SUser user)//НУЖНО ОТРЕДАКТИТЬ
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: UserC/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: UserC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,IsMananger,Email,PhoneNumber")] SUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: UserC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UserClaims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UserC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.UserClaims.FindAsync(id);
            _context.UserClaims.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            //return _context.UserClaim.Any(e => e.Id == id);
            return _context.Users.Contains(new SUser { Id = id });//might be bug
        }
    }
}