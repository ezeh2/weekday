using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeekDayWebApplication.Models;
using System.IO;

namespace WeekDayWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DateTime dt = DateTime.Now;
            CalculateModel calculateModel = new CalculateModel();
            calculateModel.Message = "Bitte geben Sie ein Datum ein.";
            calculateModel.Day = dt.Day;
            calculateModel.Month = dt.Month;
            calculateModel.Year = dt.Year;
            return View("Index", calculateModel);
        }

        [HttpPost]
        public IActionResult Calculate(CalculateModel calculateModel)
        {
            if (!this.ModelState.IsValid)
            {
                calculateModel.Message = "invalid";
            }
            else
            {
                try
                {
                    DateTime dt = new DateTime(calculateModel.Year.GetValueOrDefault(0),
                        calculateModel.Month.GetValueOrDefault(0),
                        calculateModel.Day.GetValueOrDefault(0)
                        );


                    DayOfWeek dow = dt.DayOfWeek;
                    calculateModel.Message = dow.ToString();
                }
                catch (ArgumentException ex)
                {
                    calculateModel.Message = ex.Message;
                }
            }

            return View("Index", calculateModel);
        }

        public IActionResult PwdHash()
        {
            return View("PwdHash");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SaveData()
        {
            string cd = Directory.GetCurrentDirectory();
            ViewData["CurrentDirectory"] = cd;
            ViewData["RequestPath"] = this.Request.Path.Value;

            using (FileStream fs = System.IO.File.Open("SaveData.txt", FileMode.Append))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"### {DateTime.Now.ToString("yyyy-MM-dd HH:mm.s")}");
                    foreach(string key in this.Request.Query.Keys)
                    {
                        string value = this.Request.Query[key];
                        sw.WriteLine(key);
                        sw.WriteLine(value);
                        sw.WriteLine("===");
                    }
                    sw.WriteLine("###");
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
