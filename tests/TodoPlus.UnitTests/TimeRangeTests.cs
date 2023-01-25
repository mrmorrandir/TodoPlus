using TodoPlus.Parsers.Todos;

namespace TodoPlus.UnitTests;

public class TimeRangeTests
{
    [Fact]
    public void ShouldCreateEstimate_WithContructor()
    {
        var est = new TimeRange(1, 2, 3, 4, 5, 6);
        
        est.Years.Should().Be(1);
        est.Weeks.Should().Be(2);
        est.Days.Should().Be(3);
        est.Hours.Should().Be(4);
        est.Minutes.Should().Be(5);
        est.Seconds.Should().Be(6);
    }
    
    [Fact]
    public void ShouldCreateAndRecalculateEstimate_WithContructor()
    {
        var est = new TimeRange(0, 51, 6, 23, 59, 60);
        
        est.Years.Should().Be(1);
        est.Weeks.Should().Be(0);
        est.Days.Should().Be(0);
        est.Hours.Should().Be(0);
        est.Minutes.Should().Be(0);
        est.Seconds.Should().Be(0);
    }

    [Fact]
    public void GetTimeSpan_ShouldReturnCorrectTimeSpan()
    {
        var est = new TimeRange(1, 2, 3, 4, 5, 6);
        
        est.GetTimeSpan().Should().Be(new TimeSpan(365 * 1 + 7 * 2 + 3, 4, 5, 6));
    }
    
    [Theory]
    [InlineData("1y 2w 3d 4h 5m 6s", 1, 2, 3, 4, 5, 6)]
    [InlineData("1y 2w 3d 4h 5m", 1, 2, 3, 4, 5, 0)]
    [InlineData("1y 2w 3d 4h", 1, 2, 3, 4, 0, 0)]
    [InlineData("1y 2w 3d", 1, 2, 3, 0, 0, 0)]
    [InlineData("1y 2w", 1, 2, 0, 0, 0, 0)]
    [InlineData("1y", 1, 0, 0, 0, 0, 0)]
    [InlineData("2w 3d 4h 5m 6s", 0, 2, 3, 4, 5, 6)]
    [InlineData("2w 3d 4h 5m", 0, 2, 3, 4, 5, 0)]
    [InlineData("2w 3d 4h", 0, 2, 3, 4, 0, 0)]
    [InlineData("2w 3d", 0, 2, 3, 0, 0, 0)]
    [InlineData("2w", 0, 2, 0, 0, 0, 0)]
    [InlineData("3d 4h 5m 6s", 0, 0, 3, 4, 5, 6)]
    [InlineData("3d 4h 5m", 0, 0, 3, 4, 5, 0)]
    [InlineData("3d 4h", 0, 0, 3, 4, 0, 0)]
    [InlineData("3d", 0, 0, 3, 0, 0, 0)]
    [InlineData("4h 5m 6s", 0, 0, 0, 4, 5, 6)]
    [InlineData("4h 5m", 0, 0, 0, 4, 5, 0)]
    [InlineData("4h", 0, 0, 0, 4, 0, 0)]
    [InlineData("5m 6s", 0, 0, 0, 0, 5, 6)]
    [InlineData("5m", 0, 0, 0, 0, 5, 0)]
    [InlineData("6s", 0, 0, 0, 0, 0, 6)]
    public void Parse_ShouldReturnEstimate_WhenInputIsValid(string input, int years, int weeks, int days, int hours, int minutes, int seconds)
    {
        var est = TimeRange.Parse(input);
        
        est.Years.Should().Be(years);
        est.Weeks.Should().Be(weeks);
        est.Days.Should().Be(days);
        est.Hours.Should().Be(hours);
        est.Minutes.Should().Be(minutes);
        est.Seconds.Should().Be(seconds);
    }
}