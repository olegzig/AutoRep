using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Controllers
{
    public class UserC : Controller
    {
        private readonly AuthContext _context;
        private readonly UserManager<SUser> _userManager;
        private readonly SignInManager<SUser> _signInManager;
        public UserC(AuthContext context, UserManager<SUser> userManager, SignInManager<SUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Details(string id)
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
        public async Task<IActionResult> Create([Bind("Id,UserName,IsMananger,Email,PhoneNumber,Password,ConfirmPassword")] SUser user)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(user);
                //await _context.SaveChangesAsync();

                var newUser = new SUser { UserName = user.Email, Email = user.Email, IsMananger = user.IsMananger, PhoneNumber = user.PhoneNumber };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
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
                    var oldUser = await _userManager.FindByIdAsync(id);
                    oldUser.Email = user.Email;
                    oldUser.PhoneNumber = user.PhoneNumber;
                    oldUser.IsMananger = user.IsMananger;
                    oldUser.UserName = user.UserName;
                    var Result = await _userManager.UpdateAsync(oldUser);
                    if (!Result.Succeeded)
                    {
                        //return NotFound(value:Result.Errors.FirstOrDefault().Description);//Я не знаю работает ли это, но да.

                        foreach (var error in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }
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
            await _signInManager.RefreshSignInAsync(user);
            return View(user);
        }

        // GET: UserC/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: UserC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return NotFound(value: result.Errors.ToString());//Я не знаю работает ли это, но да.
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            //return _context.UserClaim.Any(e => e.Id == id);
            return _context.Users.Contains(new SUser { Id = id });//might be bug
        }
    }
}