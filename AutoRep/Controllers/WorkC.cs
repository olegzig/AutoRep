using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Controllers
{
    [Authorize]
    public class WorkC : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public WorkC(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            Configuration = config;
        }

        // GET: WorkC
        public async Task<IActionResult> Index(string searchString, Work.SortState sortOrder = Work.SortState.ClientAsc)
        {
            IQueryable<Work> works = _context.Work;
            ViewBag.SearchString = searchString;
            if (searchString != null)
                works = works.Where(x => x.Client == searchString);

            ViewData["ClientSort"] = sortOrder == Work.SortState.ClientDesc ? Work.SortState.ClientAsc : Work.SortState.ClientDesc;
            ViewData["DateSort"] = sortOrder == Work.SortState.DateDesc ? Work.SortState.DateAsc : Work.SortState.DateDesc;

            works = sortOrder switch
            {
                Work.SortState.ClientDesc => works.OrderByDescending(x => x.Client),
                Work.SortState.DateAsc => works.OrderBy(x => x.Date.Date),
                Work.SortState.DateDesc => works.OrderByDescending(x => x.Date.Date),
                _ => works.OrderBy(x => x.Client),
            };
            return View(await works.AsNoTracking().ToListAsync());
        }

        // GET: WorkC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            ViewData["SelectedUser"] = GetUsersList().FirstOrDefault(x => x.Id == work.Worker.ToString()).UserName;
            ViewData["SelectedWorkType"] = GetWorkTypeList().FirstOrDefault(x => x.Id == Convert.ToInt32(work.WorkType)).Name;//Я таким образом показываю имя пользователя и тип работы. Просто не трогай

            return View(work);
        }

        // GET: WorkC/Create
        // make a viewbug of workers
        public List<SUser> GetUsersList()
        {
            var connection = Configuration.GetConnectionString("AuthContextConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [Id], [UserName] from [AspNetUsers]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<SUser> users = new List<SUser>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    users.Add(new SUser { Id = Convert.ToString(idr[0]), UserName = Convert.ToString(idr[1]) });
                }
            }

            con.Close();
            ViewBag.UsersBag = users;
            return users;
        }

        // make a viewbug of workTypes
        public List<WorkType> GetWorkTypeList()
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [id],[name] from [WorkType]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<WorkType> workTypes = new List<WorkType>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    workTypes.Add(new WorkType { Id = Convert.ToInt32(idr["Id"]), Name = Convert.ToString(idr["Name"]) });
                }
            }

            con.Close();
            ViewBag.WorkTypeBag = workTypes;
            return workTypes;
        }

        // GET: WorkC/Create
        public IActionResult Create()
        {
            GetUsersList();
            GetWorkTypeList();
            return View();
        }

        // GET: WorkC/CreateOn/5
        public async Task<IActionResult> CreateOn(int? id)//Тут мы создаём на остовании заявки. Передаём через viewBag
        {
            GetUsersList();
            GetWorkTypeList();
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Request.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            ViewBag.Name = work.ContactData;
            return View();
        }

        // POST: WorkC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Client,Worker,Date,WorkType")] Work work)
        {
            if (ModelState.IsValid)
            {
                //User user = new User();
                //string[] selectedWorkerString = Request.Form["SelectedWorker"].ToString().Split(' ');
                //user.Id = Int32.Parse(selectedWorkerString[0]);
                //work.Worker = user.Id;

                _context.Add(work);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(work);
        }

        // GET: WorkC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Это (2 нижестоящие строки) в принципе не должно работать, но оно работает и славненько, ок?
            GetUsersList();
            GetWorkTypeList();
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            return View(work);
        }

        // POST: WorkC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Client,Date,Worker,WorkType")] Work work)
        {
            if (id != work.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(work);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkExists(work.Id))
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
            return View(work);
        }

        // GET: WorkC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }
            ViewData["SelectedUser"] = GetUsersList().FirstOrDefault(x => x.Id == work.Worker.ToString()).UserName;
            ViewData["SelectedWorkType"] = GetWorkTypeList().FirstOrDefault(x => x.Id == Convert.ToInt32(work.WorkType)).Name;

            return View(work);
        }

        // POST: WorkC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var work = await _context.Work.FindAsync(id);
            _context.Work.Remove(work);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkExists(int id)
        {
            return _context.Work.Any(e => e.Id == id);
        }
    }
}//just for repair