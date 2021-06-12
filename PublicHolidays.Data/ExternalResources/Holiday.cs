using System.Collections.Generic;

namespace PublicHolidays.Data.ExternalResources
{
    public class Holiday
    {
        public Date Date { get; set; }

        public IEnumerable<TranslatedHolidayName> TranslatedNames { get; set; }
        public string HolidayType { get; set; }

    }
}