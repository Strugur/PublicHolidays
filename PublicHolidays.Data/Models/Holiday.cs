using System;
using System.Collections.Generic;
using System.Text;

namespace PublicHolidays.Data.Models
{
    public class Holiday
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public virtual IEnumerable<HolidayName> Names { get; set; }
        public int DateId { get; set; }
        public int CountryId { get; set; }
        public Date Date { get; set; }
        public string Type { get; set; }


    }
}
