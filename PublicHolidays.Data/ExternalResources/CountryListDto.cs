using System.Collections.Generic;

namespace PublicHolidays.Data.ExternalResources
{
     public class CountryListDto
    {
        public string Error {get;set;}
        public IEnumerable<Country> Payload {get;set;} 
    } 
}
