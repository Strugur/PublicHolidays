using System;
using System.Collections.Generic;
using System.Text;

namespace PublicHolidays.Data.Models
{
    public class Country
    {
        public int Id { get;set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public virtual SupportedDate FromDate { get; set; }
        public virtual SupportedDate ToDate { get; set; }
        public virtual IEnumerable<Region> Regions { get; set; }
        public virtual IEnumerable<Holiday> Holidays { get; set; }
    }
}
