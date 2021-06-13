using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PublicHolidays.Data.Models
{
    public class Holiday
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public virtual IEnumerable<HolidayName> Names { get; set; }
        [Required]
        public int DateId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public Date Date { get; set; }
        public string Type { get; set; }


    }
}
