using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration Configuration;
        private readonly AuthContext _userContext;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, ApplicationDbContext context, AuthContext usercontext)
        {
            _context = context;
            _logger = logger;
            Configuration = config;
            _userContext = usercontext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            GetWorkTypeList();
            return View();
        }

        public IActionResult Statistic()
        {
            return View();
        }

        #region WorkTypeCountChart

        public ActionResult VisualizeWorKTypeCountResult()
        {
            return Json(WorKTypeCountResult());
        }

        public List<WorkType> WorKTypeCountResult()
        {
            for (int i = 1; i < _context.Work.Count(); i++)//i идём по списку work
            {
                if (_context.Work.Any(x => x.Id == i))//если в work существует элемент i
                {
                    string a = _context.Work.First(x => x.Id == i).WorkType;//записываем чё там
                    for (int z = 0; z <= _context.WorkType.Select(x => x.Id).Max(); z++)//z идём по worktype.id, c 0 до макс id
                    {
                        if (a.Contains(z.ToString()))//если есть - увеличиваем
                        {
                            _context.WorkType.First(x => x.Id == z).countusage++;
                        }
                    }
                }
            }
            List<WorkType> lst = _context.WorkType.ToList();

            return lst;
        }

        #endregion WorkTypeCountChart

        #region WorkTypeCostChart

        public ActionResult VisualizeWorKTypeCostResult()
        {
            return Json(WorKTypeCostResult());
        }

        public List<WorkType> WorKTypeCostResult()
        {
            List<WorkType> lst = _context.WorkType.ToList();
            return lst;
        }

        #endregion WorkTypeCostChart

        #region WorkerCountChart

        public ActionResult VisualizeWorkerCountResult()
        {
            return Json(WorkerCountResult());
        }

        public List<SUser> WorkerCountResult()
        {
            string[] idArr = _context.Work.Select(x => x.Worker).ToArray();

            foreach (string i in idArr)
            {
                _userContext.Users.First(x => x.Id == i).count++;
            }

            string[] idArrForNames = _userContext.Users.Select(x => x.Id).ToArray();

            foreach (string i in idArrForNames)
            {
                _userContext.Users.First(x => x.Id == i).name = _userContext.Users.First(x => x.Id == i).UserName;
            }

            List<SUser> lst = _userContext.Users.ToList();

            return lst;
        }

        #endregion WorkerCountChart

        #region MachinePartCountChart

        public ActionResult VisualizeMachinePartCountResult()
        {
            return Json(MachinePartCountResult());
        }

        public List<MachineParts> MachinePartCountResult()
        {
            for (int i = 1; i < _context.MachineParts.Count(); i++)//i идём по списку work
            {
                if (_context.MachineParts.Any(x => x.Id == i))//если в work существует элемент i
                {
                    string a = _context.Work.First(x => x.Id == i).MachineParts;//записываем чё там
                    for (int z = 0; z <= _context.MachineParts.Select(x => x.Id).Max(); z++)//z идём по worktype.id, c 0 до макс id
                    {
                        if (a.Contains(z.ToString()))//если есть - увеличиваем
                        {
                            _context.MachineParts.First(x => x.Id == z).count++;
                        }
                    }
                }
            }
            List<MachineParts> lst = _context.MachineParts.ToList();

            return lst;
        }

        #endregion WorkTypeCountChart

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserRequest request)
        {
            GetWorkTypeList();
            if (ModelState.IsValid)
            {
                request.WorkType = string.Join(",", request.WorkTypeIds);
                _context.Add(request);
                await _context.SaveChangesAsync();
                if (!User.Identity.IsAuthenticated)
                {
                    ModelState.Clear();
                    return View("../Home/Index");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public IActionResult Privacy()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}