using TodoPlus.Parsers.Todos;

namespace TodoPlus.UnitTests.Parsers.Todos;

public class TodoParserTests
{
    [Theory]
    [InlineData("✔ Aufgabe @MEB1 @est(1h) @done(23-01-09 08:19)")]
    [InlineData("☐ Aufgabe @MEB1")]
    [InlineData("✔ Aufgabe @MEB1 @NAA1 @started(23-01-11 13:47) @done(23-01-11 13:50) @lasted(3m34s)")]
    [InlineData("✘ Aufgabe @MEB1 @started(23-01-12 09:14) @cancelled(23-01-12 09:32) @wasted(18m28s)")]
    public void TryParse_ShouldParseTodo_WhenTextIsValid(string line)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("☐ Aufgabe 1", State.Pending)]
    [InlineData("✔ Aufgabe 2 @done(23-01-12 08:53)", State.Done)]
    [InlineData("✘ Aufgabe 3 @cancelled(23-01-12 08:53)", State.Cancelled)]
    public void TryParse_ShouldReturnState_WhenTextIsValid(string line, State state)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(state);
        todo.Description.Should().StartWith("Aufgabe");
    }
    
    [Theory]
    [InlineData("☐ Aufgabe 4 @started(23-01-12 08:53)")]
    public void TryParse_ShouldReturnDates_WhenStartedIsSet(string line)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(State.Pending);
        todo.Started.Should().Be(new DateTime(2023, 01, 12, 8, 53, 0));
    }

    [Theory]
    [InlineData("✔ Aufgabe 2 @done(23-01-12 08:53)")]
    public void TryParse_ShouldReturnDates_WhenDoneIsSet(string line)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(State.Done);
        todo.Done.Should().Be(new DateTime(2023, 01, 12, 8, 53, 0));
    }
    
    [Theory]
    [InlineData("✘ Aufgabe 3 @cancelled(23-01-12 08:53)")]
    public void TryParse_ShouldReturnDates_WhenCancelledIsSet(string line)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(State.Cancelled);
        todo.Cancelled.Should().Be(new DateTime(2023, 01, 12, 8, 53, 0));
    }
    
    [Theory]
    [InlineData("☐ Aufgabe 1 @NAA1 @MEB1", new[]{"NAA1", "MEB1"})]
    public void TryParse_ShouldReturnCustomTags_WhenAvailable(string line, string[] tags)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(State.Pending);
        foreach (var tag in tags)
            todo.Tags.Should().Contain(tag);
    }
    
    [Theory]
    [InlineData("☐ Aufgabe 1 @at(NAA1) @at(MEB1)", new[]{"NAA1", "MEB1"})]
    public void TryParse_ShouldReturnCustomProperties_WhenAvailable(string line, string[] properties)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(State.Pending);
        foreach (var property in properties)
            todo.Properies.Should().Contain(p => p.Value == property);
    }
    
    [Theory]
    [InlineData("✔ Aufgabe 1 @started(21-02-02 11:34) @done(23-01-12 09:37) @lasted(1y49w22h3m5s)", 1, 49, 0, 22, 3, 5)]
    public void TryParse_ShouldReturnDuration_WhenAvailable(string line, int years, int weeks, int days, int hours, int minutes, int seconds)
    {
        var todoParser = new TodoParser();

        var result = todoParser.TryParse(line, out var todo);

        result.Should().BeTrue();
        todo.State.Should().Be(State.Done);
        todo.Lasted.HasValue.Should().BeTrue();
        todo.Lasted.Value.Years.Should().Be(years);
        todo.Lasted.Value.Weeks.Should().Be(weeks);
        todo.Lasted.Value.Days.Should().Be(days);
        todo.Lasted.Value.Hours.Should().Be(hours);
        todo.Lasted.Value.Minutes.Should().Be(minutes);
        todo.Lasted.Value.Seconds.Should().Be(seconds);
    }
}