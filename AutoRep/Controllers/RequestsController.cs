using AutoRep.Data;
using AutoRep.Models;
using AutoRep.Services;

using Microsoft.AspNetCore.Authorization;
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
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public RequestsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            Configuration = config;
        }

        // GET: Requests
        public async Task<IActionResult> Index(int? page, UserRequest.SortState sortOrder = Models.UserRequest.SortState.ClientAsc)
        {
            ViewBag.CurrentSort = sortOrder;
            IQueryable<UserRequest> requests = _context.Request;

            ViewData["NameSort"] = sortOrder == Models.UserRequest.SortState.ClientDesc ? Models.UserRequest.SortState.ClientAsc : Models.UserRequest.SortState.ClientDesc;

            requests = sortOrder switch
            {
                UserRequest.SortState.ClientDesc => requests.OrderByDescending(x => x.Name),
                _ => requests.OrderBy(x => x.Name),
            };

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(await requests.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
            //return View(await requests.AsNoTracking().ToListAsync());
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            GetWorkTypeList();
            return View();
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

        //make a string of worktypes that used in this work
        public string GetWorkTypeListString(string[] WTIds)
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
                    if (WTIds.Contains(idr["id"].ToString()))
                        workTypes.Add(new WorkType { Id = Convert.ToInt32(idr["Id"]), Name = Convert.ToString(idr["Name"]) });
                }
            }

            con.Close();
            string x = String.Join(", ", workTypes.Select(x => x.Name));
            return x;
        }

        // make a viewbug of machineParts
        public string GetmachinePartsListString(string[] MPIds)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [id],[name] from [MachineParts]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<MachineParts> machineParts = new List<MachineParts>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    if (MPIds.Contains(idr["id"].ToString()))
                        machineParts.Add(new MachineParts { Id = Convert.ToInt32(idr["Id"]), Name = Convert.ToString(idr["Name"]) });
                }
            }

            con.Close();
            return String.Join(", ", machineParts.Select(x => x.Name));
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkType,Name,PhoneNumber,Email,WorkTypeIds,Message")] UserRequest request)
        {
            GetWorkTypeList();
            if (ModelState.IsValid)
            {
                request.WorkType = string.Join(",", request.WorkTypeIds);
                _context.Add(request);
                await _context.SaveChangesAsync();

                //sending email
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(
                    request.Email,
                   "Автомастерская",
                    $"Здравствуйте {request.Name}.\n" +
                    $"Уведомляем вас, что вы отправили запрос на {GetmachinePartsListString(request.WorkTypeIds)}.");

                if (!User.Identity.IsAuthenticated)
                {
                    ModelState.Clear();
                    return View("../Home/Index");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetWorkTypeList();
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            request.WorkTypeIds = request.WorkType.Split(',');
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkType,Name,PhoneNumber,Email,WorkTypeIds,Message")] UserRequest request)
        {
            GetWorkTypeList();
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                request.WorkType = string.Join(",", request.WorkTypeIds);
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
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
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            request.WorkTypeIds = request.WorkType.Split(',');
            ViewData["SelectedWorkType"] = GetWorkTypeListString(request.WorkTypeIds);

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Request.FindAsync(id);
            _context.Request.Remove(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.Id == id);
        }
    }
}