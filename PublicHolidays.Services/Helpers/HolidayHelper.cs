using System.Collections.Generic;

namespace PublicHolidays.Services.Helpers
{
    public static class HolidayHelper
    {
        public static IEnumerable<int> AddWeekends(IEnumerable<int> holidaysByDayOfWeek)
        {
            var items = new List<int>(holidaysByDayOfWeek);
            
              for (int i = 0; i < items.Count; i++)
            {
                var nextIndex = i + 1;
                var nextOfNextIndex = i + 2;

                if (items[i] == 5)
                {
                    if (items[i + 1] != 6 && items[i+1] != 7 && items[i + 2] != 7 )
                    {
                        items.Insert(nextIndex, 6);
                        items.Insert(nextOfNextIndex, 7);
                    }
                }
                if(items[i] == 5 && items[i+1] == 7)
                {
                    items.Insert(nextIndex, 6);
                }
                if (items[i] == 6 && items[i + 1] != 7)
                {
                    items.Insert(nextIndex, 7);
                }
            }
            return items;
        }

        public static int GetMaxFreeDays(IEnumerable<int> freeDaysByDayOfWeek)
        {
            var items = new List<int>(freeDaysByDayOfWeek);
            int count1 = 1;
            int count2 = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if((items[i + 1] - items[i]) == 1)
                {
                    // dasdasdas
                }
            }
            return count1;
        }
    }
}