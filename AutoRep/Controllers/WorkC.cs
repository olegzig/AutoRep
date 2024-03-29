﻿using AutoRep.Data;
using AutoRep.Models;
using AutoRep.Services;

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
    [Authorize]
    public class WorkC : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;
        private readonly UserManager<SUser> _userManager;

        public WorkC(ApplicationDbContext context, IConfiguration config, UserManager<SUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            Configuration = config;
        }

        // GET: WorkC
        public async Task<IActionResult> Index(int? page, string searchString, string currentFilter, bool showOutdated, bool showAll, Work.SortState sortOrder = Work.SortState.ClientAsc)
        {
            IQueryable<Work> works = _context.Work;

            ViewBag.ShowAll = showAll == true ? "checked" : "unchecked";//контроль вида
            ViewBag.ShowOutdated = showOutdated == true ? "checked" : "unchecked";//контроль вида

            ViewBag.CurrentFilter = searchString;
            ViewBag.SearchString = searchString;
            ViewBag.CurrentSort = sortOrder;

            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
                ViewBag.CurrentFilter = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
                works = works.Where(x => x.Client.Contains(searchString));

            works = showAll switch
            {
                true => works,
                false => works.Where(x => x.Worker == _userManager.GetUserId(HttpContext.User)),
            };
            works = showOutdated switch
            {
                true => works,
                false => works.Where(x => x.IsCompleted == false),
            };

            ViewData["ClientSort"] = sortOrder == Work.SortState.ClientDesc ? Work.SortState.ClientAsc : Work.SortState.ClientDesc;
            ViewData["DateSort"] = sortOrder == Work.SortState.DateDesc ? Work.SortState.DateAsc : Work.SortState.DateDesc;
            works = sortOrder switch
            {
                Work.SortState.ClientDesc => works.OrderByDescending(x => x.Client),
                Work.SortState.DateAsc => works.OrderBy(x => x.Date.Date),
                Work.SortState.DateDesc => works.OrderByDescending(x => x.Date.Date),
                _ => works.OrderBy(x => x.Client),
            };

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await works.AsNoTracking().ToPagedListAsync(pageNumber, pageSize));
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

            ViewData["SelectedUser"] = GetUsersList().FirstOrDefault(x => x.Id == work.Worker).UserName;
            //ViewData["SelectedMachinePart"] = GetmachinePartsList().FirstOrDefault(x => x.Id == Convert.ToInt32(work.MachineParts)).Name;
            work.MachinePartsIds = work.MachineParts.Split(',');
            ViewData["SelectedMachinePart"] = GetmachinePartsListString(work.MachinePartsIds);
            //ViewData["SelectedWorkType"] = GetWorkTypeList().FirstOrDefault(x => x.Id == Convert.ToInt32(work.WorkType)).Name;//Я таким образом показываю имя пользователя и тип работы. Просто не трогай
            work.WorkTypeIds = work.WorkType.Split(',');
            ViewData["SelectedWorkType"] = GetWorkTypeListString(work.WorkTypeIds);
            return View(work);
        }

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
            SqlCommand cmd = new SqlCommand("select [id],[name],[cost] from [WorkType]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<WorkType> workTypes = new List<WorkType>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    workTypes.Add(new WorkType { Id = Convert.ToInt32(idr["Id"]), Name = Convert.ToString(idr["Name"]) + " | " + Convert.ToString(idr["Cost"]) + " BYN " });
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
            return String.Join(", ", workTypes.Select(x => x.Name));
        }

        //make cost of machineparts
        public double GetWorkTypeCost(string[] MPIds)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [id],[cost] from [WorkType]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<MachineParts> WorkTypes = new List<MachineParts>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    if (MPIds.Contains(idr["id"].ToString()))
                        WorkTypes.Add(new MachineParts { Id = Convert.ToInt32(idr["Id"]), Cost = Convert.ToDouble(idr["Cost"]) });
                }
            }

            con.Close();
            return WorkTypes.Sum(x => x.Cost);
        }

        // make a viewbug of machineParts
        public List<MachineParts> GetmachinePartsList()
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [id],[name],[cost],[count] from [MachineParts]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<MachineParts> machineParts = new List<MachineParts>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    machineParts.Add(new MachineParts { Id = Convert.ToInt32(idr["Id"]), Name = Convert.ToString(idr["Name"]) + " | " + Convert.ToString(idr["Cost"]) + " BYN " + " | " + Convert.ToString(idr["Count"]) + " ШТ. " });
                }
            }

            con.Close();
            ViewBag.MachinePartsBag = machineParts;
            return machineParts;
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

        //make cost of machineparts
        public double GetmachinePartsCost(string[] MPIds)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [id],[cost] from [MachineParts]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<MachineParts> machineParts = new List<MachineParts>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    if (MPIds.Contains(idr["id"].ToString()))
                        machineParts.Add(new MachineParts { Id = Convert.ToInt32(idr["Id"]), Cost = Convert.ToDouble(idr["Cost"]) });
                }
            }

            con.Close();
            return machineParts.Sum(x => x.Cost);
        }

        //make cost of machineparts
        public bool IsMachinePartsListCountIsNull(string[] MPIds)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand("select [id],[count] from [MachineParts]", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();

            List<MachineParts> machineParts = new List<MachineParts>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {
                    if (MPIds.Contains(idr["id"].ToString()))
                        if(Convert.ToDouble(idr["Count"]) < 0)
                        {
                            return true;
                        }
                }
            }

            con.Close();
            return false;
        }

        //changes machine parts counts for Create() and Edit()
        public void ChangeMachinePartsCount(string[] MPIds)
        {

            foreach(string id in MPIds)
            {
                if (_context.MachineParts.Any(x => x.Id.ToString() == id))
                {
                    _context.MachineParts.First(z => z.Id == int.Parse(id)).Count--;
                }
            }

            _context.SaveChanges();
        }
        public void ChangeBackMachinePartsCount(string[] MPIds)
        {
            foreach (string id in MPIds)
            {
                if (_context.MachineParts.Any(x => x.Id.ToString() == id))
                {
                    _context.MachineParts.First(z => z.Id == int.Parse(id)).Count++;
                }
            }

            _context.SaveChanges();
        }

        // GET: WorkC/Create
        public IActionResult Create()
        {
            GetUsersList();
            GetWorkTypeList();
            GetmachinePartsList();
            return View();
        }

        // GET: WorkC/CreateOn/5
        public async Task<IActionResult> CreateOn(int? id)//Тут мы создаём на остовании заявки. Передаём через viewBag
        {
            GetUsersList();
            GetWorkTypeList();
            GetmachinePartsList();
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Request.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            ViewBag.Name = work.Name;
            ViewBag.MadeOnId = id;
            ViewBag.Email = work.Email;
            ViewBag.Phone = work.PhoneNumber;
            ViewBag.Work = work.WorkType.Split(',');
            ViewBag.Msg = work.Message;

            return View();
        }

        // POST: WorkC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Work work, string submit)
        {
            GetUsersList();
            GetWorkTypeList();
            GetmachinePartsList();
            if (ModelState.IsValid)
            {
                //User user = new User();
                //string[] selectedWorkerString = Request.Form["SelectedWorker"].ToString().Split(' ');
                //user.Id = Int32.Parse(selectedWorkerString[0]);
                //work.Worker = user.Id;

                //that add machine parts counter, taht we removed before
                if (work.MachineParts != null && submit == null)
                    ChangeBackMachinePartsCount(work.MachineParts.Split(','));
                if (work.MachineParts != null && submit == "Я уверен в своём выборе!")
                    ChangeBackMachinePartsCount(work.MachineParts.Split(','));
                if (work.MachineParts != null && IsMachinePartsListCountIsNull(work.MachinePartsIds))
                    ChangeBackMachinePartsCount(work.MachineParts.Split(','));

                work.WorkType = string.Join(",", work.WorkTypeIds);
                work.MachineParts = string.Join(",", work.MachinePartsIds);
                work.Cost = GetmachinePartsCost(work.MachinePartsIds) + GetWorkTypeCost(work.WorkTypeIds);
                _context.Add(work);
                

                ChangeMachinePartsCount(work.MachinePartsIds);//that edit machine parts counts after select machine parts

                //if count < 0, we are undo all actions
                if (IsMachinePartsListCountIsNull(work.MachinePartsIds) && submit != "Я уверен в своём выборе!")
                {
                    ChangeBackMachinePartsCount(work.MachinePartsIds);
                    ViewBag.JavaScriptFunction = "!";
                    _context.Remove(work);
                    return View();
                }

                //send email
                if (work.MadeOnId != null)
                {
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(
                        _context.Request.Find(work.MadeOnId).Email,
                       "Автомастерская",
                        $"Здравствуйте.\n" +
                        $"Уведомляем вас, что вы записаны на {work.Date} к {GetUsersList().Find(x => x.Id == work.Worker).UserName}.");

                    _context.Request.Remove(_context.Request.Find(work.MadeOnId));
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(work);
        }

        // GET: WorkC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Это в принципе не должно работать, но оно работает и славненько, ок?
            GetUsersList();
            GetWorkTypeList();
            GetmachinePartsList();
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Work.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            work.WorkTypeIds = work.WorkType.Split(',');
            work.MachinePartsIds = work.MachineParts.Split(',');
            work.Cost = GetmachinePartsCost(work.MachinePartsIds) + GetWorkTypeCost(work.WorkTypeIds);
            return View(work);
        }

        // POST: WorkC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Work work, string submit)
        {
            GetUsersList();
            GetWorkTypeList();
            GetmachinePartsList();
            if (id != work.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //that add machine parts counter, taht we removed before
                if (work.MachineParts != null && submit == null)
                    ChangeBackMachinePartsCount(work.MachineParts.Split(','));
                if (work.MachineParts != null && submit == "Я уверен в своём выборе!")
                    ChangeBackMachinePartsCount(work.MachineParts.Split(','));
                if (work.MachineParts != null && !IsMachinePartsListCountIsNull(work.MachinePartsIds))
                    ChangeBackMachinePartsCount(work.MachineParts.Split(','));

                work.WorkType = string.Join(",", work.WorkTypeIds);
                work.MachineParts = string.Join(",", work.MachinePartsIds);
                work.Cost = GetmachinePartsCost(work.MachinePartsIds) + GetWorkTypeCost(work.WorkTypeIds);
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
                ChangeMachinePartsCount(work.MachinePartsIds);//that edit machine parts counts after select machine parts

                //if count < 0, we are undo all actions
                if (IsMachinePartsListCountIsNull(work.MachinePartsIds) && submit != "Я уверен в своём выборе!")
                {
                    ChangeBackMachinePartsCount(work.MachinePartsIds);
                    ViewBag.JavaScriptFunction = "!";
                    return View();
                }
                
                _context.Update(work);
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
            ViewData["SelectedUser"] = GetUsersList().FirstOrDefault(x => x.Id == work.Worker).UserName;
            //ViewData["SelectedMachinePart"] = GetmachinePartsList().FirstOrDefault(x => x.Id == Convert.ToInt32(work.MachineParts)).Name;
            work.MachinePartsIds = work.MachineParts.Split(',');
            ViewData["SelectedMachinePart"] = GetmachinePartsListString(work.MachinePartsIds);
            //ViewData["SelectedWorkType"] = GetWorkTypeList().FirstOrDefault(x => x.Id == Convert.ToInt32(work.WorkType)).Name;
            work.WorkTypeIds = work.WorkType.Split(',');
            ViewData["SelectedWorkType"] = GetWorkTypeListString(work.WorkTypeIds);

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