
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp
{
    public static class BEDayOfWeek
    {
        public static DayOfWeekBE BeDayOfWeek(this DateTime time)
        {
            switch(time.DayOfWeek)
            {
                case DayOfWeek.Monday: return DayOfWeekBE.Maandag;
                case DayOfWeek.Tuesday: return DayOfWeekBE.Dinsdag;
                case DayOfWeek.Wednesday: return DayOfWeekBE.Woensdag;
                case DayOfWeek.Thursday: return DayOfWeekBE.Donderdag;
                case DayOfWeek.Friday: return DayOfWeekBE.Vrijdag;
                case DayOfWeek.Saturday: return DayOfWeekBE.Zaterdag;
                case DayOfWeek.Sunday: return DayOfWeekBE.Zondag;
            }
            return DayOfWeekBE.Onbekend;
        }
    }

    public enum DayOfWeekBE{
        // Summary:
        //     Indicates Sunday.
        Zondag = 0,
        //
        // Summary:
        //     Indicates Monday.
        Maandag = 1,
        //
        // Summary:
        //     Indicates Tuesday.
        Dinsdag= 2,
        //
        // Summary:
        //     Indicates Wednesday.
        Woensdag = 3,
        //
        // Summary:
        //     Indicates Thursday.
        Donderdag = 4,
        //
        // Summary:
        //     Indicates Friday.
        Vrijdag = 5,
        //
        // Summary:
        //     Indicates Saturday.
        Zaterdag = 6,
        Onbekend = -1
    }
}