﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PublicHolidays.Data.Models
{
    public class Date
    {
        public int Id { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int DayOfWeek { get; set; }

    }
}
