using TodoPlus.Parsers.Files;
using TodoPlus.Parsers.Projects;
using TodoPlus.Parsers.Todos;

namespace TodoPlus.UnitTests.Parsers.Files;

public class TodoFileParserTests
{
    [Fact]
    public void ShouldReturnFile_WhenTextIsValid()
    {
        var content = System.IO.File.ReadAllText("./Test.todo");
        var fileParser = new TodoFileParser(new ProjectParser(), new TodoParser());
        
        var func = ()=> fileParser.Parse(content);
        var result = func.Should().NotThrow().Subject;

        result.Projects.Should().HaveCount(3);
        result.Todos.Should().HaveCount(14);
    }
    
    [Fact]
    public void ShouldReturnTodos_WithCorrectProject()
    {
        var content = System.IO.File.ReadAllText("./Test.todo");
        var fileParser = new TodoFileParser(new ProjectParser(), new TodoParser());
        
        var func = ()=> fileParser.Parse(content);
        var result = func.Should().NotThrow().Subject;

        result.Todos.First().Project.Should().BeNull();
        result.Todos.Skip(5).Should().AllSatisfy(t => t.Project.Should().NotBeNull());

        result.Todos[6].Project.ToString().Should().Be("Project 1");
        result.Todos[9].Project.ToString().Should().Be("Project 1 > Project 1.1");
        result.Todos[12].Project.ToString().Should().Be("Project 1 > Project 1.2");
    }
}