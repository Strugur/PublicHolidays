using System.Collections;
using System.Collections.Generic;

namespace PublicHolidays.Services.Helpers
{
    public static class HolidayHelper
    {
        private static readonly  IDictionary<string,int> _weekDay = new Dictionary<string, int> {
            {"Monday",1},
            {"Thuesday",2},
            {"Wednesday",3},
            {"Thursday",4},
            {"Friday",5},
            {"Saturday",6},
            {"Saturday2",6},
            {"Sunday",7}
        };
      
        public static IEnumerable<int> AddWeekends(IEnumerable<int> holidaysByDayOfWeek)
        {
            var items = new List<int>(holidaysByDayOfWeek);

            for (int i = 0; i < items.Count - 1; i++)
            {
                var nextIndex = i + 1;
                var nextOfNextIndex = i + 2;
                var next = items[i + 1];

                if (items[i] == 5)
                {
                    if (next != _weekDay["Saturday"] && next != _weekDay["Sunday"] && items[i + 2] != _weekDay["Sunday"])
                    {
                        items.Insert(nextIndex, _weekDay["Saturday"]);
                        items.Insert(nextOfNextIndex, _weekDay["Sunday"]);
                    }
                }
                if (items[i] == _weekDay["Friday"]&& next == _weekDay["Sunday"])
                {
                    items.Insert(nextIndex, _weekDay["Saturday"]);
                }
                if (items[i] == _weekDay["Saturday"] && next != _weekDay["Sunday"])
                {
                    items.Insert(nextIndex, _weekDay["Sunday"]);
                }
            }
            return items;
        }

        public static int GetMaxFreeDays(IEnumerable<int> freeDaysByDayOfWeek)
        {
            var items = new List<int>(freeDaysByDayOfWeek);
            int finalCount = 1;
            int workCount = 1;

            int difference = 0;

            for (int i = 0; i < items.Count - 1; i++)
            {
                var current = items[i];
                var next = items[i + 1];

                if (current > next && current == 7)
                {
                    difference = current + next;
                }
                difference = next - current;
                
                if (difference == 1 || difference == 8)
                {
                    workCount++;
                }
                else
                {
                    if (workCount > finalCount)
                    {
                        finalCount = workCount;
                    }
                    workCount = 1;
                }

            }
            return finalCount;
        }
    }
}