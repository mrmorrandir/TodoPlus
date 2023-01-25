using TodoPlus.Parsers.Projects;

namespace TodoPlus.UnitTests.Parsers.Projects;

public class ProjectParserTests
{
    [Theory]
    [InlineData("MyProject:")]
    [InlineData("    MyProject:")]
    [InlineData("MyProject: ")]
    [InlineData("MyProject:\r\n")]
    [InlineData("    MyProject: \r\n")]
    public void TryParse_ShouldParseProject_WhenTextIsValid(string text)
    {
        var parser = new ProjectParser();
        var result = parser.TryParse(text, out var project);

        result.Should().BeTrue();
        project.Name.Should().Be("MyProject");
    }
    
    [Theory]
    [InlineData("MyProject: Test")]
    [InlineData("    MyProject: Test")]
    [InlineData("MyProject:Test\r\n")]
    public void TryParse_ShouldNotParseProject_WhenTextIsInValid(string text)
    {
        var parser = new ProjectParser();
        var result = parser.TryParse(text, out var project);

        result.Should().BeFalse();
        project.Should().BeNull();
    }
}