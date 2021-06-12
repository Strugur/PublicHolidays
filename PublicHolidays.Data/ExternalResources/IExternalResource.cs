using System.Collections.Generic;
using System.Threading.Tasks;


namespace PublicHolidays.Data.ExternalResources
{
   public interface IExternalResource 
    {
        Task<CountryListDto> GetCountryList();
        Task<HolidayListDto> GetHolidaysForYear(int year, string countryCode);
        Task<CheckDateDto> CheckIfIsWorkDay(string date, string countryCode);
    }
}