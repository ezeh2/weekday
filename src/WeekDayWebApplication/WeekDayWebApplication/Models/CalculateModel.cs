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
        public int? Day { get; set; }
        [Required]
        public int? Month { get; set; }
        [Required]
        public int? Year { get; set; }

        public string Message { get; set; }
    }
}
