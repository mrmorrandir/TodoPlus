namespace TodoPlus.Parsers.Todos
{
    public interface ITodoParser
    {
        bool TryParse(string text, out Todo todo);
    }
}