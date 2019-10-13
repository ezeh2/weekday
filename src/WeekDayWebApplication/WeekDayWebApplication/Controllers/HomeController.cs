using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeekDayWebApplication.Models;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
