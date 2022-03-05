using AutoRep.Data;
using AutoRep.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Configuration;
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

        public HomeController(ILogger<HomeController> logger, IConfiguration config, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            Configuration = config;
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

        [HttpPost]
        public JsonResult AjaxMethod()
        {
            string query = "SELECT ShipCity, COUNT(orderid) TotalOrders";
            query += " FROM Orders WHERE ShipCountry = 'USA' GROUP BY ShipCity";
            string constr = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
                            {
                            "ShipCity", "TotalOrders"
                            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                            sdr["ShipCity"], sdr["TotalOrders"]
                            });
                        }
                    }

                    con.Close();
                }
            }

            return Json(chartData);
        }

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