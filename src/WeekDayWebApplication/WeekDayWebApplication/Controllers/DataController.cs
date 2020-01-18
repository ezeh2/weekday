using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeekDayWebApplication.Controllers
{
    public class DataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowData()
        {
            string cd = Directory.GetCurrentDirectory();
            ViewData["CurrentDirectory"] = cd;
            ViewData["RequestPath"] = this.Request.Path.Value;

            return View();
        }

        public IActionResult SaveData()
        {
            using (FileStream fs = System.IO.File.Open(@"wwwroot\Data.txt", FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"### {DateTime.Now.ToString("yyyy-MM-dd HH:mm.s")}");
                    foreach (string key in this.Request.Query.Keys)
                    {
                        string value = this.Request.Query[key];
                        sw.WriteLine(key);
                        sw.WriteLine(value);
                        sw.WriteLine("===");
                    }
                    sw.WriteLine("###");
                }
            }

            return Ok();
        }
    }
}
