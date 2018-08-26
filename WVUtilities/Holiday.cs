using System;
using System.Collections.Generic;
using System.Linq;


namespace WVUtilities
{
    public class HolidayUtility
    {
        HashSet<DateTime> m_holidays = null;

        public HolidayUtility(int year)
        {
            m_holidays = GetHolidays(year - 1);
            m_holidays.UnionWith(GetHolidays(year));
            m_holidays.UnionWith(GetHolidays(year + 1));
        }
        public DateTime GetPreviousBusinessDay(DateTime dataDate)
        {
            dataDate = this.AdjustForWeekend(dataDate);
            if (m_holidays.Contains(dataDate))
            {
                dataDate = dataDate.AddDays(-1);
                dataDate = this.GetPreviousBusinessDay(dataDate);
            }

            return dataDate;
        }

        // Function to get the next business day.
        public DateTime GetNextBusinessDay(DateTime dataDate, bool include)
        {
            if (include)
                dataDate = this.AdjustForWeekendForward(dataDate);
            else
            {
                dataDate = dataDate.AddDays(1);
                dataDate = this.AdjustForWeekendForward(dataDate);
            }

            if (m_holidays.Contains(dataDate))
            {
                dataDate = this.GetNextBusinessDay(dataDate, false);
            }
            return dataDate;
        }
        public DateTime GetNthBusinessDay(DateTime dataDate, int n, bool include)
        {
            // Get a valid today or tomorrow based on include.
            dataDate = GetNextBusinessDay(dataDate, include);
            // for loop n times
            for (int i = 0; i < n - 1; i++)
                dataDate = GetNextBusinessDay(dataDate, false);

            return dataDate;
        }
        // Ends here.

        public HashSet<DateTime> GetHolidays(int year)
        {
            HashSet<DateTime> holidays = new HashSet<DateTime>();

            //NEW YEARS 
            DateTime newYearsDate = AdjustForWeekendHoliday(new DateTime(year, 1, 1).Date);
            holidays.Add(newYearsDate);

            //MARTIN LUTHER KING DAY -- Third monday in January
            DateTime MLKDay = new DateTime(year, 1, 21);
            DayOfWeek dayOfWeek = MLKDay.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                MLKDay = MLKDay.AddDays(-1);
                dayOfWeek = MLKDay.DayOfWeek;
            }
            holidays.Add(MLKDay.Date);


            //PRESIDENT's DAY -- Third monday in February
            DateTime PresDay = new DateTime(year, 2, 21);
            dayOfWeek = PresDay.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                PresDay = PresDay.AddDays(-1);
                dayOfWeek = PresDay.DayOfWeek;
            }
            holidays.Add(PresDay.Date);

            //GOOD FRIDAY/EASTER HOLIDAY -- This is in March
            DateTime GoodFriday = EasterSunday(year).AddDays(-2);
            holidays.Add(GoodFriday);

            //MEMORIAL DAY  -- Last monday in May 
            DateTime memorialDay = new DateTime(year, 5, 31);
            dayOfWeek = memorialDay.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                memorialDay = memorialDay.AddDays(-1);
                dayOfWeek = memorialDay.DayOfWeek;
            }
            holidays.Add(memorialDay.Date);

            //INDEPENCENCE DAY 
            DateTime independenceDay = AdjustForWeekendHoliday(new DateTime(year, 7, 4).Date);
            holidays.Add(independenceDay);

            //LABOR DAY -- 1st Monday in September 
            DateTime laborDay = new DateTime(year, 9, 1);
            dayOfWeek = laborDay.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                laborDay = laborDay.AddDays(1);
                dayOfWeek = laborDay.DayOfWeek;
            }
            holidays.Add(laborDay.Date);

            //THANKSGIVING DAY - 4th Thursday in November 
            var thanksgiving = (from day in Enumerable.Range(1, 30)
                                where new DateTime(year, 11, day).DayOfWeek == DayOfWeek.Thursday
                                select day).ElementAt(3);
            DateTime thanksgivingDay = new DateTime(year, 11, thanksgiving);
            holidays.Add(thanksgivingDay.Date);

            //CHRISTMAS --25th December    
            DateTime christmasDay = AdjustForWeekendHoliday(new DateTime(year, 12, 25).Date);
            holidays.Add(christmasDay);
            return holidays;
        }

        public DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int yearMod19 = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * yearMod19 + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - yearMod19) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        public DateTime AdjustForWeekendHoliday(DateTime holiday)
        {
            if (holiday.DayOfWeek == DayOfWeek.Saturday)
            {
                return holiday.AddDays(-1);
            }
            else if (holiday.DayOfWeek == DayOfWeek.Sunday)
            {
                return holiday.AddDays(1);
            }
            else
            {
                return holiday;
            }
        }

        public DateTime AdjustForWeekend(DateTime holiday)
        {
            if (holiday.DayOfWeek == DayOfWeek.Saturday)
                return holiday.AddDays(-1);
            else if (holiday.DayOfWeek == DayOfWeek.Sunday)
                return holiday.AddDays(-2);
            else
                return holiday;
        }

        // This function will return the next businessday after the weekend.
        /* Ex: For a saturday/sunday it returns monday */
        public DateTime AdjustForWeekendForward(DateTime holiday)
        {
            if (holiday.DayOfWeek == DayOfWeek.Saturday)
                return holiday.AddDays(2);
            else if (holiday.DayOfWeek == DayOfWeek.Sunday)
                return holiday.AddDays(1);
            else
                return holiday;
        }
    }
}
