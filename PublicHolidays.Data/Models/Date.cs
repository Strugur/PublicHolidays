using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PublicHolidays.Data.Models
{
    public class Date
    {
        public int Id { get; set; }
        [Required]
        [Range(1,31)]
        public int Day { get; set; }
        [Required]
        [Range(1,12)]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [Range(1,7)]
        public int DayOfWeek { get; set; }

    }
}
