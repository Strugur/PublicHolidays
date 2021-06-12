using System.Collections.Generic;

namespace PublicHolidays.Data.ExternalResources
{
     public class HolidayListDto
    {
        public string Error {get;set;}
        public IEnumerable<Holiday> Payload {get;set;} 
    } 
}
