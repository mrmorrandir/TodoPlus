namespace TodoPlus.Parsers.Projects
{
    public interface IProjectParser
    {
        bool TryParse(string text, out Project project);
    }
}