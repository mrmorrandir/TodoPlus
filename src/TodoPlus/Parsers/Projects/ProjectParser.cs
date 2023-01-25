using System.Text.RegularExpressions;

namespace TodoPlus.Parsers.Projects
{
    public class ProjectParser : IProjectParser
    {
        private static readonly Regex _projectRegex = new Regex("^(?<indentation>[\t ]*)(?<name>.*):[ \r]*$", RegexOptions.Compiled);
            
        public ProjectParser()
        {
            
        }

        public bool TryParse(string text, out Project project)
        {
            var match = _projectRegex.Match(text);
            if (!match.Success)
            {
                project = null;
                return false;
            }
            
            project = new Project
            {
                Name = match.Groups["name"].Value,
                Indentation = match.Groups["indentation"].Value.Length
            };
            return true;
        }
    }
}