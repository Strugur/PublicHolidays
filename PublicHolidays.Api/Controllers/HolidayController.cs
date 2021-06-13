using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PublicHolidays.Api.Contracts;
using PublicHolidays.Data.ExternalResources;
using PublicHolidays.Services;


namespace PublicHolidays.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class HolidayController : ControllerBase
    {
       

        private readonly ILogger<HolidayController> _logger;
        private readonly HolidayService _holidayService;

        public HolidayController(ILogger<HolidayController> logger, HolidayService holidayService)
        {
            _logger = logger;
            _holidayService = holidayService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<string> GetList(int year,string country)
        {   
            var dtoFromholidayService =  await _holidayService.GetListForYear(year,country);

            // return dtoFromholidayService;
            if(!string.IsNullOrEmpty(dtoFromholidayService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromholidayService.Error
                };
                return JsonConvert.SerializeObject(err);
            }

            var holidays = dtoFromholidayService.Payload
                .GroupBy(h => h.Date.Month);
            return JsonConvert.SerializeObject(holidays);
           
        }
        
        [HttpGet]
        [Route("maxFreeDaysInARow")]

        public async Task<string> MaxFreeDaysInARow (string country, int year)
        {

            var dtoFromholidayService =  await _holidayService.GetMaxFreeDaysInARow(country,year);

            // return dtoFromholidayService;
            if(!string.IsNullOrEmpty(dtoFromholidayService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromholidayService.Error
                };
                return JsonConvert.SerializeObject(err);
            }

            
            return JsonConvert.SerializeObject(new MaxFreeDaysInARowResponse()
            {
                maxFreeDaysInARow = dtoFromholidayService.MaxFreeDaysInARow
            });
            // return JsonConvert.SerializeObject(dtoFromholidayService);

            
        } 
    }
  
}
