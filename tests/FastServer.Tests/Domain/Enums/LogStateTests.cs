using FastServer.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace FastServer.Tests.Domain.Enums;

public class LogStateTests
{
    [Fact]
    public void LogState_ShouldHaveCorrectValues()
    {
        // Assert
        ((int)LogState.Pending).Should().Be(0);
        ((int)LogState.InProgress).Should().Be(1);
        ((int)LogState.Completed).Should().Be(2);
        ((int)LogState.Failed).Should().Be(3);
        ((int)LogState.Timeout).Should().Be(4);
        ((int)LogState.Cancelled).Should().Be(5);
    }

    [Fact]
    public void LogState_ShouldHaveSixStates()
    {
        // Arrange
        var states = Enum.GetValues<LogState>();

        // Assert
        states.Should().HaveCount(6);
    }

    [Theory]
    [InlineData("Pending", LogState.Pending)]
    [InlineData("InProgress", LogState.InProgress)]
    [InlineData("Completed", LogState.Completed)]
    [InlineData("Failed", LogState.Failed)]
    [InlineData("Timeout", LogState.Timeout)]
    [InlineData("Cancelled", LogState.Cancelled)]
    public void LogState_ShouldParseFromString(string stateName, LogState expected)
    {
        // Act
        var result = Enum.Parse<LogState>(stateName);

        // Assert
        result.Should().Be(expected);
    }
}

public class DataSourceTypeTests
{
    [Fact]
    public void DataSourceType_ShouldHaveCorrectValues()
    {
        // Assert
        ((int)DataSourceType.PostgreSQL).Should().Be(0);
        ((int)DataSourceType.SqlServer).Should().Be(1);
    }

    [Fact]
    public void DataSourceType_ShouldHaveTwoTypes()
    {
        // Arrange
        var types = Enum.GetValues<DataSourceType>();

        // Assert
        types.Should().HaveCount(2);
    }
}
