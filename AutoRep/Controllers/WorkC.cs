using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Controllers
{
    public class WorkC : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkC(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkC
        public async Task<IActionResult> Index()
        {
            return View(await _context.Work.ToListAsync());
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

            ViewData["SelectedUser"] = GetUsersList().FirstOrDefault(x => x.Id == work.Worker).Name;
            ViewData["SelectedWorkType"] = GetWorkTypeList().FirstOrDefault(x => x.Id == work.WorkType).Name;

            return View(work);
        }

        // GET: WorkC/Create
        // make a viewbug of workers
        public List<User> GetUsersList()
        {
            var connection = "Server=(localdb)\\mssqllocaldb;Database=AutoRepDB;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [Id], [Name] from [User]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<User> users = new List<User>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    users.Add(new User { Id = Convert.ToInt32(idr["Id"]), Name = Convert.ToString(idr["Name"]) });
                }
            }

            con.Close();
            ViewBag.UsersBag = users;
            return users;
        }

        // make a viewbug of workTypes
        public List<WorkType> GetWorkTypeList()
        {
            var connection = "Server=(localdb)\\mssqllocaldb;Database=AutoRepDB;Trusted_Connection=True;MultipleActiveResultSets=true";
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
            ViewData["SelectedUser"] = GetUsersList().FirstOrDefault(x => x.Id == work.Worker).Name;
            ViewData["SelectedWorkType"] = GetWorkTypeList().FirstOrDefault(x => x.Id == work.WorkType).Name;

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
}