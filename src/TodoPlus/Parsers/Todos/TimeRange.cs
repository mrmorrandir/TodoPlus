using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TodoPlus.Parsers.Todos
{
    public struct TimeRange
    {
        private static readonly Regex _timeYearsRegex =
            new Regex(@"(?<years>\d+)[ ]*(?:y|year|years)", RegexOptions.Compiled);
        private static readonly Regex _timeWeeksRegex =
            new Regex(@"(?<weeks>\d+)[ ]*(?:w|week|weeks)", RegexOptions.Compiled);
        private static readonly Regex _timeDaysRegex =
            new Regex(@"(?<days>\d+)[ ]*(?:d|day|days)", RegexOptions.Compiled);
        private static readonly Regex _timeHoursRegex =
            new Regex(@"(?<hours>\d+)[ ]*(?:h|hour|hours)", RegexOptions.Compiled);
        private static readonly Regex _timeMinutesRegex =
            new Regex(@"(?<minutes>\d+)[ ]*(?:m|min|minute|minutes)", RegexOptions.Compiled);
        private static readonly Regex _timeSecondsRegex =
            new Regex(@"(?<seconds>\d+)[ ]*(?:s|sec|second|seconds)", RegexOptions.Compiled);

        public int Years { get; private set; }
        public int Weeks { get; private set; }
        public int Days { get; private set; }
        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public TimeRange(int years, int weeks, int days, int hours, int minutes, int seconds)
        {
            Years = years;
            Weeks = weeks;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Recalculate();
        }

        public TimeSpan GetTimeSpan()
        {
            return new TimeSpan(Years * 365 + Weeks * 7 + Days, Hours, Minutes, Seconds);
        }

        public static TimeRange FromTimeSpan(TimeSpan timeSpan)
        {
            var years = timeSpan.Days / 365;
            var weeks = (timeSpan.Days - years * 365) / 7;
            var days = timeSpan.Days - years * 365 - weeks * 7;
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;
            return new TimeRange(years, weeks, days, hours, minutes, seconds);
        }

        public static TimeRange Parse(string estimate)
        {
            var est = new TimeRange();
            var yearsMatch = _timeYearsRegex.Match(estimate);
            if (yearsMatch.Success)
                est.Years = int.Parse(yearsMatch.Groups["years"].Value);

            var weeksMatch = _timeWeeksRegex.Match(estimate);
            if (weeksMatch.Success)
                est.Weeks = int.Parse(weeksMatch.Groups["weeks"].Value);

            var daysMatch = _timeDaysRegex.Match(estimate);
            if (daysMatch.Success)
                est.Days = int.Parse(daysMatch.Groups["days"].Value);

            var hoursMatch = _timeHoursRegex.Match(estimate);
            if (hoursMatch.Success)
                est.Hours = int.Parse(hoursMatch.Groups["hours"].Value);

            var minutesMatch = _timeMinutesRegex.Match(estimate);
            if (minutesMatch.Success)
                est.Minutes = int.Parse(minutesMatch.Groups["minutes"].Value);

            var secondsMatch = _timeSecondsRegex.Match(estimate);
            if (secondsMatch.Success)
                est.Seconds = int.Parse(secondsMatch.Groups["seconds"].Value);
            est.Recalculate();
            return est;
        }

        private void Recalculate()
        {
            if (Seconds >= 60)
            {
                Minutes += Seconds / 60;
                Seconds = Seconds % 60;
            }

            if (Minutes >= 60)
            {
                Hours += Minutes / 60;
                Minutes = Minutes % 60;
            }

            if (Hours >= 24)
            {
                Days += Hours / 24;
                Hours = Hours % 24;
            }

            if (Days >= 7)
            {
                Weeks += Days / 7;
                Days = Days % 7;
            }

            if (Weeks >= 52)
            {
                Years += Weeks / 52;
                Weeks = Weeks % 52;
            }
        }

        public override string ToString()
        {
            var est = new StringBuilder();
            if (Years > 0)
                est.Append($"{Years}y ");
            if (Weeks > 0)
                est.Append($"{Weeks}w ");
            if (Days > 0)
                est.Append($"{Days}d ");
            if (Hours > 0)
                est.Append($"{Hours}h ");
            if (Minutes > 0)
                est.Append($"{Minutes}m ");
            if (Seconds > 0)
                est.Append($"{Seconds}s ");
            return est.ToString().Trim();
        }
    }
}