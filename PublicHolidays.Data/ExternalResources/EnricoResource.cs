using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicHolidays.Data.ExternalResources
{
    public class EnricoResource : IExternalResource
    {
        private readonly string _enricoUrl = "https://kayaposoft.com/enrico/json/v2.0";

        public async Task<CountryListDto> GetCountryList()
        {
            var action = "getSupportedCountries";
            var dto = new CountryListDto();

            var res = await _enricoUrl
                .SetQueryParam("action", action)
                .GetAsync()
                .ReceiveJson<dynamic>()
                .ConfigureAwait(false);

            if (res.GetType().ToString() == "Newtonsoft.Json.Linq.JArray")
            {
                var resJArray = (JArray)res;
                var payload = new List<Country>();
                foreach (var e in resJArray)
                {
                    var item = new Country()
                    {
                        CountryCode = (string)e["countryCode"],
                        Regions = (string[])e["regions"].Select(region => (string)region).ToArray(),
                        FullName = (string)e["fullName"],
                        FromDate = new SupportedDate()
                        {
                            Day = (int)e["fromDate"]["day"],
                            Month = (int)e["fromDate"]["month"],
                            Year = (int)e["fromDate"]["year"],
                        },
                        ToDate = new SupportedDate()
                        {
                            Day = (int)e["toDate"]["day"],
                            Month = (int)e["toDate"]["month"],
                            Year = (int)e["toDate"]["year"],
                        },
                        // Date = new Date()
                        // {
                        //     Day = (int)e["date"]["day"],
                        //     Month = (int)e["date"]["month"],
                        //     Year = (int)e["date"]["year"],
                        //     DayOfWeek = (int)e["date"]["dayOfWeek"],
                        // }
                    };
                    payload.Add(item);
                }
                dto.Error = "";
                dto.Payload = payload;
                return dto;
                
            }
            var resJObj = (JObject)res;
            dto.Error = (string)resJObj["error"];
            return dto;
        }

        public async Task<HolidayListDto> GetHolidaysForYear(int year, string countryCode)
        {
            var dto = new HolidayListDto();
            var action = "getHolidaysForYear";

            var res = await _enricoUrl
                .SetQueryParam("action", action)
                .SetQueryParam("year", year)
                .SetQueryParam("country", countryCode)
                .SetQueryParam("holidayType", "public_holiday")
                .GetAsync()
                .ReceiveJson<dynamic>()
                .ConfigureAwait(false);

            if (res.GetType().ToString() == "Newtonsoft.Json.Linq.JArray")
            {
                var resJArray = (JArray)res;
                var payload = new List<Holiday>();
                foreach (var e in resJArray)
                {
                    var item = new Holiday()
                    {
                        Date = new Date()
                        {
                            Day = (int)e["date"]["day"],
                            Month = (int)e["date"]["month"],
                            Year = (int)e["date"]["year"],
                            DayOfWeek = (int)e["date"]["dayOfWeek"],
                        },
                        TranslatedNames = e["name"]
                            .Select(n => new TranslatedHolidayName()
                            {
                                Lang = (string)n["lang"],
                                Text = (string)n["text"]
                            }).ToList(),
                        HolidayType = (string)e["holidayType"]
                    
                    };
                    payload.Add(item);
                }
                dto.Error = "";
                dto.Payload = payload;
                return dto;

            }
            var resJObj = (JObject)res;
            dto.Error = (string)resJObj["error"];
            return dto;
        }
    
        public async Task<CheckDateDto> CheckIfIsWorkDay(string date, string countryCode)
        {
            var action = "isWorkDay";
            var dto = new CheckDateDto();
            var res = await _enricoUrl
                .SetQueryParam("action", action)
                .SetQueryParam("date", date)
                .SetQueryParam("country", countryCode)
                .GetAsync()
                .ReceiveJson<dynamic>()
                .ConfigureAwait(false);

            var resJObj = (JObject)res;
            if(!string.IsNullOrEmpty((string)resJObj["error"]))
            {
                dto.Error = (string)resJObj["error"];
            }else
            {
                dto.Error = "";
                dto.IsWorkDay = (bool)resJObj["isWorkDay"];
            }

            return dto;
        }    
    }
}