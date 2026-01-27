using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace FastServer.Tests.Domain.Entities;

public class LogServicesHeaderTests
{
    [Fact]
    public void LogServicesHeader_ShouldInitialize_WithDefaultValues()
    {
        // Arrange & Act
        var header = new LogServicesHeader();

        // Assert
        header.LogId.Should().Be(0);
        header.LogMethodUrl.Should().BeEmpty();
        header.LogMicroservices.Should().NotBeNull();
        header.LogMicroservices.Should().BeEmpty();
        header.LogServicesContents.Should().NotBeNull();
        header.LogServicesContents.Should().BeEmpty();
    }

    [Fact]
    public void LogServicesHeader_ShouldSetProperties_Correctly()
    {
        // Arrange
        var header = new LogServicesHeader();
        var dateIn = DateTime.UtcNow;
        var dateOut = dateIn.AddSeconds(5);

        // Act
        header.LogId = 1;
        header.LogDateIn = dateIn;
        header.LogDateOut = dateOut;
        header.LogState = LogState.Completed;
        header.LogMethodUrl = "/api/test";
        header.LogMethodName = "TestMethod";
        header.MicroserviceName = "TestService";
        header.UserId = "user123";
        header.TransactionId = "trans456";
        header.HttpMethod = "POST";
        header.RequestDuration = 500;

        // Assert
        header.LogId.Should().Be(1);
        header.LogDateIn.Should().Be(dateIn);
        header.LogDateOut.Should().Be(dateOut);
        header.LogState.Should().Be(LogState.Completed);
        header.LogMethodUrl.Should().Be("/api/test");
        header.LogMethodName.Should().Be("TestMethod");
        header.MicroserviceName.Should().Be("TestService");
        header.UserId.Should().Be("user123");
        header.TransactionId.Should().Be("trans456");
        header.HttpMethod.Should().Be("POST");
        header.RequestDuration.Should().Be(500);
    }

    [Fact]
    public void LogServicesHeader_ShouldHandleNullableProperties()
    {
        // Arrange
        var header = new LogServicesHeader();

        // Act & Assert
        header.LogMethodName.Should().BeNull();
        header.LogFsId.Should().BeNull();
        header.MethodDescription.Should().BeNull();
        header.ErrorCode.Should().BeNull();
        header.ErrorDescription.Should().BeNull();
        header.RequestDuration.Should().BeNull();
    }

    [Fact]
    public void LogServicesHeader_ShouldAddLogMicroservices()
    {
        // Arrange
        var header = new LogServicesHeader { LogId = 1 };
        var microservice = new LogMicroservice
        {
            LogId = 1,
            LogMicroserviceText = "Test log text"
        };

        // Act
        header.LogMicroservices.Add(microservice);

        // Assert
        header.LogMicroservices.Should().HaveCount(1);
        header.LogMicroservices.First().LogMicroserviceText.Should().Be("Test log text");
    }

    [Fact]
    public void LogServicesHeader_ShouldAddLogServicesContents()
    {
        // Arrange
        var header = new LogServicesHeader { LogId = 1 };
        var content = new LogServicesContent
        {
            LogId = 1,
            LogServicesContentText = "Test content",
            ContentNo = "001"
        };

        // Act
        header.LogServicesContents.Add(content);

        // Assert
        header.LogServicesContents.Should().HaveCount(1);
        header.LogServicesContents.First().LogServicesContentText.Should().Be("Test content");
        header.LogServicesContents.First().ContentNo.Should().Be("001");
    }
}
