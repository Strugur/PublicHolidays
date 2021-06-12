using System.Collections.Generic;

namespace PublicHolidays.Services.Dtos
{
 public class GetMaxFreeDaysInARowDto
 {
     public string Error {get;set;}
     public int MaxFreeDaysInARow {get;set;}
     public IEnumerable<int> Days {get;set;}
     public IEnumerable<int> Days2 {get;set;}
 } 
}