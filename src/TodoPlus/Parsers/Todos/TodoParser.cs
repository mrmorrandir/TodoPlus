using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TodoPlus.Parsers.Todos
{
    public class TodoParser : ITodoParser
    {
        private static readonly Regex _todoRegex = new Regex("^(?<indentation>[\t ]*)(?<state>☐|✔|✘)[ ]+(?<description>.*)$", RegexOptions.Compiled);
        private static readonly Regex _cancelledRegex =
            new Regex(@"@cancelled\((?<datetime>(?<year>\d{2})-(?<month>\d{2})-(?<day>\d{2}) (?<hour>\d{2}):(?<minute>\d{2}))\)", RegexOptions.Compiled);
        private static readonly Regex _doneRegex =
            new Regex(@"@done\((?<datetime>(?<year>\d{2})-(?<month>\d{2})-(?<day>\d{2}) (?<hour>\d{2}):(?<minute>\d{2}))\)", RegexOptions.Compiled);
        private static readonly Regex _startedRegex =
            new Regex(@"@started\((?<datetime>(?<year>\d{2})-(?<month>\d{2})-(?<day>\d{2}) (?<hour>\d{2}):(?<minute>\d{2}))\)", RegexOptions.Compiled);
        private static readonly Regex _estRegex =
            new Regex(@"@est\((?<est>.*)\)", RegexOptions.Compiled);
        private static readonly Regex _lastedRegex =
            new Regex(@"@lasted\((?<lasted>.*)\)", RegexOptions.Compiled);
        private static readonly Regex _wastedRegex =
            new Regex(@"@wasted\((?<wasted>.*)\)", RegexOptions.Compiled);
        private static readonly Regex _customTagRegex =
            new Regex(@"@(?!est)(?!done)(?!cancelled)(?!started)(?!lasted)(?!wasted)(?<tag>\w*)", RegexOptions.Compiled);
        private static readonly Regex _customPropertyRegex =
            new Regex(@"@(?!est)(?!done)(?!cancelled)(?!started)(?!lasted)(?!wasted)(?<name>\w*)\((?<value>[^\)]+)\)");
        
        public bool TryParse(string text, out Todo todo)
        {
            todo = null;
            var match = _todoRegex.Match(text);
            if (!match.Success) return false;

            todo = new Todo();
            switch (match.Groups["state"].Value)
            {
                case "☐":
                    todo.State = State.Pending;
                    break;
                case "✔":
                    todo.State = State.Done;
                    break;
                case "✘":
                    todo.State = State.Cancelled;
                    break;
            }
            var description = match.Groups["description"].Value;
            todo.Description = description;
            todo.Indentation = match.Groups["indentation"].Value.Length;
            var startedMatch = _startedRegex.Match(description);
            if (startedMatch.Success)
            {
                DateTime.TryParseExact(startedMatch.Groups["datetime"].Value, "yy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime started);
                todo.Started = started;
            }
            
            var doneMatch = _doneRegex.Match(description);
            if (doneMatch.Success)
            {
                DateTime.TryParseExact(doneMatch.Groups["datetime"].Value, "yy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime done);
                todo.Done = done;
            }
            
            var cancelledMatch = _cancelledRegex.Match(description);
            if (cancelledMatch.Success)
            {
                DateTime.TryParseExact(cancelledMatch.Groups["datetime"].Value, "yy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime cancelled);
                todo.Cancelled = cancelled;
            }

            var lastedMatch = _lastedRegex.Match(description);
            if (lastedMatch.Success)
                todo.Lasted = TimeRange.Parse(lastedMatch.Groups["lasted"].Value);
            
            var wastedMatch = _wastedRegex.Match(description);
            if (wastedMatch.Success)
                todo.Wasted = TimeRange.Parse(wastedMatch.Groups["wasted"].Value);
            
            var estMatch = _estRegex.Match(description);
            if (estMatch.Success)
                todo.Estimate = TimeRange.Parse(estMatch.Groups["est"].Value);

            var tagMatches = _customTagRegex.Matches(description);
            for (var matchNum = 0; matchNum < tagMatches.Count; matchNum++)
            {
                if (!tagMatches[matchNum].Success) continue;
                if (todo.Tags is null)
                    todo.Tags = new List<string>();

                todo.Tags.Add(tagMatches[matchNum].Groups["tag"].Value);
            }
            
            var propertyMatches = _customPropertyRegex.Matches(description);
            for (var matchNum = 0; matchNum < propertyMatches.Count; matchNum++)
            {
                if (!propertyMatches[matchNum].Success) continue;
                if (todo.Properies is null)
                    todo.Properies = new List<Property>();

                todo.Properies.Add(new Property{ 
                    Name = propertyMatches[matchNum].Groups["name"].Value, 
                    Value = propertyMatches[matchNum].Groups["value"].Value
                });
            }
            
            return true;
        }
    }
}