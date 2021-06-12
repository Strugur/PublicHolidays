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
        // [Produces("application/json")]
        public async Task<string> GetList()
        {
            var dtoFromService =  await _service.GetList();

           if(!string.IsNullOrEmpty(dtoFromService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromService.Error
                };
                return JsonConvert.SerializeObject(err);
            }

            
            return JsonConvert.SerializeObject(dtoFromService.Payload);
        }

        [HttpGet]
        [Route("dayStatus")]
        public async Task<string> SpecificDayStatus(string country,string date)
        {
            var dtoFromService =   await _service.GetSpecificDayStatus(country,date);   

             if(!string.IsNullOrEmpty(dtoFromService.Error)){
                var err = new ErrorResponse()
                {
                    error = dtoFromService.Error
                };
                return JsonConvert.SerializeObject(err);
            }

            
            return JsonConvert.SerializeObject(new DayStatusResponse()
            {
                DayStatus = dtoFromService.DayStatus
            });
        }
    }
}
