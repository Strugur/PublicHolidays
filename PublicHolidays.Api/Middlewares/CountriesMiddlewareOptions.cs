using PublicHolidays.Services;

namespace PublicHolidays.Api.Middlewares
{
    public class CountriesMiddlewareOptions
    {
        public CountryService countryService {get;set;}
    }
}

