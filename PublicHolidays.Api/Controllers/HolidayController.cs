using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Holiday>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        public async Task<IActionResult> GetList(int year,string country)
        {   
            var dtoFromholidayService =  await _holidayService.GetListForYear(year,country);

            if(!string.IsNullOrEmpty(dtoFromholidayService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromholidayService.Error
                };
                
                return BadRequest(err);
            }

            var holidays = dtoFromholidayService.Payload;
            
            return Ok(holidays);
        }
        
        [HttpGet]
        [Route("maxFreeDaysInARow")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MaxFreeDaysInARowResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        public async Task<IActionResult> MaxFreeDaysInARow (string country, int year)
        {
            var dtoFromholidayService =  await _holidayService.GetMaxFreeDaysInARow(country,year);
           
            if(!string.IsNullOrEmpty(dtoFromholidayService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromholidayService.Error
                };
                return BadRequest(err);
            }

            var okResponse = new MaxFreeDaysInARowResponse()
            {
                maxFreeDaysInARow = dtoFromholidayService.MaxFreeDaysInARow
            };
           
            return Ok(okResponse);
            
        } 
    }
  
}
