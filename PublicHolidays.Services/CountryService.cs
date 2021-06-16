using System;
using PublicHolidays.Data.ExternalResources;
using PublicHolidays.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PublicHolidays.Services
{
    public class CountryService
    {
        private readonly PublicHolidaysContext _dbContext;
        private readonly IExternalResource _externalResource;

        public CountryService(PublicHolidaysContext dbContext, IExternalResource externalResource)
        {
            _dbContext = dbContext;
            _externalResource = externalResource;

        }


        public async Task<CountryListDto> GetList()
        {
            var countries = _dbContext.Countries
            .Include(c => c.FromDate)
            .Include(c => c.ToDate);

            await SaveCountriesIfNeeded();
            return new CountryListDto()
            {
                Error = "",
                Payload = countries.Select(c => new Country()
                {
                    CountryCode = c.Code,
                    FullName = c.FullName,
                    Regions = c.Regions.Select(r => r.Code).ToArray(),
                    FromDate = new SupportedDate()
                    {
                        Day = c.FromDate.Day,
                        Month = c.FromDate.Month,
                        Year = c.FromDate.Year,
                    },
                    ToDate = new SupportedDate()
                    {
                        Day = c.ToDate.Day,
                        Month = c.ToDate.Month,
                        Year = c.ToDate.Year,
                    }
                })
            };
        }

        public async Task<DayStatusDto> GetSpecificDayStatus(string countryCode, string date)
        {
            var requestDateParts = date.Split('-').Select(Int32.Parse).ToList();

            await SaveCountriesIfNeeded();

            var country = _dbContext.Countries
                .Include(c => c.FromDate)
                .Include(c => c.ToDate)
                .Include(c => c.Holidays)
                .Where(c => c.Code.Equals(countryCode))
                .SingleOrDefault();

            if (country == null)
            {
                return new DayStatusDto() { Error = $"Country {countryCode} is not supported" };
            }
            if (!Enumerable.Range(country.FromDate.Year, country.ToDate.Year).Contains(requestDateParts[2]))
            {
                return new DayStatusDto() { Error = $"Year {requestDateParts[2]} is not supported" };
            }

            if (!country.Holidays.Any())
            {
                var holidayListDto = await _externalResource.GetHolidaysForYear(requestDateParts[2], countryCode);

                if (!string.IsNullOrEmpty(holidayListDto.Error))
                {
                    return new DayStatusDto() { Error = holidayListDto.Error };
                }

                // todo
                var holidayListForDbInsert = holidayListDto.Payload
               .Select(h => new Data.Models.Holiday()
               {
                   CountryId = country.Id,
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

            var holiday = _dbContext.Holidays
                .Where(h => h.CountryId == country.Id)
                .Where(h => h.Date.Day == requestDateParts[0])
                .Where(h => h.Date.Month == requestDateParts[1])
                .Where(h => h.Date.Year == requestDateParts[2])
                .FirstOrDefault();

            if (holiday != null)
            {
                return new DayStatusDto()
                {
                    Error = "",
                    DayStatus = "Holiday"
                };
            }

            // externalResource getDayStatus
            var checkIsWorkDayDto = await _externalResource.CheckIfIsWorkDay(date, countryCode);
            if (!string.IsNullOrEmpty(checkIsWorkDayDto.Error))
            {
                return new DayStatusDto() { Error = checkIsWorkDayDto.Error };
            }
            if (checkIsWorkDayDto.IsWorkDay == true)
            {
                return new DayStatusDto()
                {
                    Error = "",
                    DayStatus = "WorkDay",
                };
            }



            return new DayStatusDto()
            {
                Error = "",
                DayStatus = "FreeDay",
            };

        }

        public async Task SaveCountriesIfNeeded()
        {
            var countries = _dbContext.Countries;

            if (!countries.Any())
            {
                var countryListDto = await _externalResource.GetCountryList();

                // if (!string.IsNullOrEmpty(countryListDto.Error))
                // {
                //     return new CountryListDto()
                //     {
                //         Error = countryListDto.Error
                //     };
                // }

                var countyListForDbInsert = countryListDto.Payload.Select(c => new Data.Models.Country()
                {
                    FullName = c.FullName,
                    Code = c.CountryCode,
                    FromDate = new Data.Models.SupportedDate()
                    {
                        Day = c.FromDate.Day,
                        Month = c.FromDate.Month,
                        Year = c.FromDate.Year,
                    },
                    ToDate = new Data.Models.SupportedDate()
                    {
                        Day = c.ToDate.Day,
                        Month = c.ToDate.Month,
                        Year = c.ToDate.Year,
                    },

                    Regions = c.Regions.Select(regionCode => new Data.Models.Region()
                    {
                        Code = regionCode
                    }).ToList()
                });

                _dbContext.Countries.AddRange(countyListForDbInsert);
                _dbContext.SaveChanges();
            }
        }





    }

}
