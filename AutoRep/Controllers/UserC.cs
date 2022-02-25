using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using X.PagedList;

namespace AutoRep.Controllers
{
    [Authorize(Roles = "mananger")]
    public class UserC : Controller
    {
        private readonly AuthContext _context;
        private readonly UserManager<SUser> _userManager;
        private readonly SignInManager<SUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleMananger;
        private readonly IConfiguration Configuration;

        public UserC(AuthContext context, UserManager<SUser> userManager, SignInManager<SUser> signInManager, RoleManager<IdentityRole> roleMananger, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _roleMananger = roleMananger;
            Configuration = config;
        }

        // GET: UserC
        public async Task<IActionResult> Index(int? page, SUser.SortState sortOrder = Models.SUser.SortState.NameAsc)
        {
            ViewBag.CurrentSort = sortOrder;
            IQueryable<SUser> users = _context.Users;

            ViewData["NameSort"] = sortOrder == Models.SUser.SortState.NameDesc ? Models.SUser.SortState.NameAsc : Models.SUser.SortState.NameDesc;

            users = sortOrder switch
            {
                Models.SUser.SortState.NameDesc => users.OrderByDescending(x => x.UserName),
                _ => users.OrderBy(x => x.UserName),
            };
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(await users.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
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
            string role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            if (role == null)
            {
                role = "none";
            }
            ViewBag.RoleBag = role;

            return View(user);
        }

        // make a viewbug of workers
        public List<IdentityRole> GetRolesList()
        {
            var connection = Configuration.GetConnectionString("AuthContextConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [Id], [Name] from [AspNetRoles]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<IdentityRole> roles = new List<IdentityRole>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    roles.Add(new IdentityRole { Id = Convert.ToString(idr[0]), Name = Convert.ToString(idr[1]) });
                }
            }

            con.Close();
            ViewBag.RolesBag = roles;
            return roles;
        }

        // GET: UserC/Create
        public IActionResult Create()
        {
            GetRolesList();
            return View();
        }

        // POST: UserC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,PhoneNumber,Password,ConfirmPassword,Role")] SUser user)
        {
            GetRolesList();
            if (ModelState.IsValid)
            {
                //_context.Add(user);
                //await _context.SaveChangesAsync();
                var newUser = new SUser { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber, Role = user.Role, Password = user.Password };
                var result = await _userManager.CreateAsync(newUser, newUser.Password);
                var result2 = await _userManager.AddToRoleAsync(newUser, _roleMananger.FindByIdAsync(user.Role).Result.Name);
                if (result.Succeeded && result2.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    foreach (var error2 in result2.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error2.Description);
                    }
                }
            }
            return View(user);
        }

        // GET: UserC/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            GetRolesList();
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PhoneNumber,Role,Password,ConfirmPassword")] SUser user)
        {
            GetRolesList();
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = await _userManager.FindByIdAsync(id);//Я щас понял, что не знаю зачем old user существеует. Работает? Ну и хер с ним
                    oldUser.Email = user.Email;
                    oldUser.PhoneNumber = user.PhoneNumber;
                    oldUser.UserName = user.UserName;
                    oldUser.Role = user.Role;
                    var Result = await _userManager.UpdateAsync(oldUser);
                    var Result2 = await _userManager.RemoveFromRolesAsync(oldUser, _userManager.GetRolesAsync(oldUser).Result);
                    var Result3 = await _userManager.AddToRoleAsync(oldUser, _roleMananger.FindByIdAsync(oldUser.Role).Result.Name);
                    if (!Result.Succeeded && !Result2.Succeeded && !Result3.Succeeded)
                    {
                        //return NotFound(value:Result.Errors.FirstOrDefault().Description);//Я не знаю работает ли это, но да.

                        foreach (var error in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        foreach (var error in Result2.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        foreach (var error in Result3.Errors)
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

            string role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            if (role == null)
            {
                role = "none";
            }
            ViewBag.RoleBag = role;

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