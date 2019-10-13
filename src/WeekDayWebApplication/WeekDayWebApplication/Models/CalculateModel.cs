using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeekDayWebApplication.Models
{
    public class CalculateModel
    {
        [Required]
        public string Day { get; set; }
        [Required]
        public string Month { get; set; }
        [Required]
        public string Year { get; set; }

        public string Message { get; set; }
    }
}
