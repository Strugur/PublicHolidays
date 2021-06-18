using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicHolidays.Services;
using Newtonsoft.Json;
using PublicHolidays.Api.Contracts.Responses;
using PublicHolidays.Api.Contracts;
using Microsoft.AspNetCore.Http;

namespace PublicHolidays.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
       

        private readonly ILogger<CountryController> _logger;
        private readonly CountryService _service;

        public CountryController(ILogger<CountryController> logger,CountryService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Data.ExternalResources.Country>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        public async Task<IActionResult> GetList()
        {
            var dtoFromService =  await _service.GetList();

           if(!string.IsNullOrEmpty(dtoFromService.Error)){
                var errMessage = new ErrorResponse()
                {
                    error = dtoFromService.Error
                };
                
                return BadRequest(errMessage);
            }
       
            return Ok(dtoFromService.Payload);
        }

        [HttpGet]
        [Route("dayStatus")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DayStatusResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [Produces("application/json")]
        public async Task<IActionResult> SpecificDayStatus(string country,string date)
        {
            var dtoFromService =   await _service.GetSpecificDayStatus(country,date);   

             if(!string.IsNullOrEmpty(dtoFromService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromService.Error
                };
                return BadRequest(err);
            }

            var dayStatusResponse = new DayStatusResponse()
            {
                DayStatus = dtoFromService.DayStatus
            };

            return Ok(dayStatusResponse);
        }
    }
}
