namespace PublicHolidays.Data.ExternalResources
{
    public class Country
    {
        public string CountryCode {get;set;}
        public string[] Regions {get;set;}
        
        public string FullName {get;set;}
        public SupportedDate FromDate {get;set;}
        public SupportedDate ToDate {get;set;}
        
    }
}