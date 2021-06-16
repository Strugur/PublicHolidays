using System;
using PublicHolidays.Data.ExternalResources;
using PublicHolidays.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PublicHolidays.Services.Dtos;
using PublicHolidays.Services.Helpers;
using Microsoft.Extensions.Logging;

namespace PublicHolidays.Services
{
    public class HolidayService
    {
        private readonly ILogger<HolidayService> _logger;
        private readonly PublicHolidaysContext _dbContext;
        private readonly IExternalResource _externalResource;
        private readonly CountryService _countryService;
        public HolidayService(
            PublicHolidaysContext dbContext,
            IExternalResource externalResource,
            CountryService countryService,
            ILogger<HolidayService> logger)
        {
            _dbContext = dbContext;
            _externalResource = externalResource;
            _countryService = countryService;
            _logger = logger;
        }

        public async Task<HolidayListDto> GetListForYear(int year, string countryCode)
        {
            await _countryService.SaveCountriesIfNeeded();

            // TODO check if country and year are supported
            var country = _dbContext.Countries
                 .Include(c => c.FromDate)
                 .Include(c => c.ToDate)
                 .Where(c => c.Code.Equals(countryCode))
                 .SingleOrDefault();

            if (country == null)
            {
                return new HolidayListDto() { Error = $"Country {countryCode} not supported" };
            }
            if (!Enumerable.Range(country.FromDate.Year, country.ToDate.Year).Contains(year))
            {
                return new HolidayListDto() { Error = $"Year {year} not supported" };
            }

            var holidays = _dbContext.Holidays
                .Where(h => h.Date.Year.Equals(year))
                .Where(h => h.CountryId.Equals(country.Id))
                .Select(h => new Holiday()
                {
                    TranslatedNames = h.Names.Select(e => new TranslatedHolidayName()
                    {
                        Lang = e.Lang,
                        Text = e.Text
                    }),
                    Date = new Date()
                    {
                        Day = h.Date.Day,
                        Month = h.Date.Month,
                        Year = h.Date.Year,
                        DayOfWeek = h.Date.DayOfWeek,
                    },
                    HolidayType = h.Type

                }).ToList();

            if (holidays.Count() != 0)
            {
                return new HolidayListDto()
                {
                    Error = "",
                    Payload = holidays
                };
            }

            var resultFromExternalResource = await _externalResource.GetHolidaysForYear(year, countryCode);

            if (!string.IsNullOrEmpty(resultFromExternalResource.Error))
            {
                return new HolidayListDto()
                {
                    Error = resultFromExternalResource.Error
                };
            }
            // TODO  - save holidays to db
            await SaveHolidays(resultFromExternalResource.Payload, country.Id);

            return resultFromExternalResource;

        }

        public async Task SaveHolidays(IEnumerable<Holiday> holidays, int countryId)
        {
            var holidayListForDbInsert = holidays
               .Select(h => new Data.Models.Holiday()
               {
                   CountryId = countryId,
                   Names = h.TranslatedNames.Select(n => new Data.Models.HolidayName()
                   {
                       Lang = n.Lang,
                       Text = n.Text
                   }).ToList(),
                   Date = new Data.Models.Date()
                   {
                       Day = h.Date.Day,
                       Month = h.Date.Month,
                       Year = h.Date.Year,
                       DayOfWeek = h.Date.DayOfWeek,
                   },
                   Type = h.HolidayType
               }).ToList();
            await _dbContext.Holidays.AddRangeAsync(holidayListForDbInsert);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<GetMaxFreeDaysInARowDto> GetMaxFreeDaysInARow(string countryCode, int year)
        {
            await _countryService.SaveCountriesIfNeeded();

            var country = _dbContext.Countries
                .Include(c => c.FromDate)
                .Include(c => c.ToDate)
                .Where(c => c.Code.Equals(countryCode))
                .SingleOrDefault();

            if (country == null)
            {
                return new GetMaxFreeDaysInARowDto() { Error = $"Country {countryCode} is not supported" };
            }
            if (!Enumerable.Range(country.FromDate.Year, country.ToDate.Year).Contains(year))
            {
                return new GetMaxFreeDaysInARowDto() { Error = $"Year {year} is not supported" };
            }

            var holidaySortedDaysOfWeek = _dbContext.Holidays
                .Where(h => h.CountryId == country.Id)
                .Where(h => h.Date.Year == year)
                .Select(h => h.Date.DayOfWeek).ToList();

            if (holidaySortedDaysOfWeek.Count == 0)
            {
                // _logger.LogInformation($"holidaySortedDaysOfWeek from db is empty");

                var fromExternalResource = await _externalResource.GetHolidaysForYear(year, countryCode);
                // _logger.LogInformation($"holidaysForYear from external resource ={fromExternalResource} ");

                if (!string.IsNullOrEmpty(fromExternalResource.Error))
                {
                    return new GetMaxFreeDaysInARowDto() { Error = fromExternalResource.Error };
                }
                await SaveHolidays(fromExternalResource.Payload, country.Id);
            }

            // _logger.LogInformation($"holidaySortedDaysOfWeek from db ={holidaySortedDaysOfWeek} ");

            var holidaySortedDaysOfWeek2 = _dbContext.Holidays
                .Where(h => h.CountryId == country.Id)
                .Where(h => h.Date.Year == year)
                .Select(h => h.Date.DayOfWeek).ToList();

            var freeDaysByDayOfWeek = HolidayHelper.AddWeekends(holidaySortedDaysOfWeek2);

            // find max count of free days 
            var maxFreeDays = HolidayHelper.GetMaxFreeDays(freeDaysByDayOfWeek);
            return new GetMaxFreeDaysInARowDto()
            {
                Days = holidaySortedDaysOfWeek,
                Days2 = freeDaysByDayOfWeek,
                MaxFreeDaysInARow = maxFreeDays
            };
        }

    }
}
