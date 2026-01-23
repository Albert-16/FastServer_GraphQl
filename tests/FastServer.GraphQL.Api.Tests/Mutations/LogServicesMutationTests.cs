using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Enums;
using FastServer.GraphQL.Api.GraphQL.Mutations;
using FastServer.GraphQL.Api.GraphQL.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastServer.GraphQL.Api.Tests.Mutations;

public class LogServicesMutationTests
{
    private readonly Mock<ILogServicesHeaderService> _mockService;
    private readonly LogServicesMutation _mutation;

    public LogServicesMutationTests()
    {
        _mockService = new Mock<ILogServicesHeaderService>();
        _mutation = new LogServicesMutation();
    }

    [Fact]
    public async Task CreateLogServicesHeader_ShouldCreateAndReturnDto()
    {
        // Arrange
        var input = new CreateLogServicesHeaderInput
        {
            LogDateIn = DateTime.UtcNow,
            LogDateOut = DateTime.UtcNow.AddSeconds(5),
            LogState = LogState.Completed,
            LogMethodUrl = "/api/test",
            LogMethodName = "TestMethod",
            HttpMethod = "POST"
        };

        var expectedDto = new LogServicesHeaderDto
        {
            LogId = 1,
            LogDateIn = input.LogDateIn,
            LogDateOut = input.LogDateOut,
            LogState = LogState.Completed,
            LogMethodUrl = "/api/test",
            LogMethodName = "TestMethod",
            HttpMethod = "POST"
        };

        _mockService.Setup(s => s.CreateAsync(
            It.IsAny<CreateLogServicesHeaderDto>(),
            null,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _mutation.CreateLogServicesHeader(_mockService.Object, input);

        // Assert
        result.Should().NotBeNull();
        result.LogId.Should().Be(1);
        result.LogMethodUrl.Should().Be("/api/test");
        result.HttpMethod.Should().Be("POST");
    }

    [Fact]
    public async Task UpdateLogServicesHeader_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var input = new UpdateLogServicesHeaderInput
        {
            LogId = 1,
            LogState = LogState.Failed,
            ErrorCode = "ERR001",
            ErrorDescription = "Test error"
        };

        var expectedDto = new LogServicesHeaderDto
        {
            LogId = 1,
            LogState = LogState.Failed,
            ErrorCode = "ERR001",
            ErrorDescription = "Test error"
        };

        _mockService.Setup(s => s.UpdateAsync(
            It.Is<UpdateLogServicesHeaderDto>(d =>
                d.LogId == 1 &&
                d.LogState == LogState.Failed &&
                d.ErrorCode == "ERR001"),
            null,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _mutation.UpdateLogServicesHeader(_mockService.Object, input);

        // Assert
        result.Should().NotBeNull();
        result.LogId.Should().Be(1);
        result.LogState.Should().Be(LogState.Failed);
        result.ErrorCode.Should().Be("ERR001");
    }

    [Fact]
    public async Task DeleteLogServicesHeader_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(1, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _mutation.DeleteLogServicesHeader(_mockService.Object, 1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteLogServicesHeader_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteAsync(999, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _mutation.DeleteLogServicesHeader(_mockService.Object, 999);

        // Assert
        result.Should().BeFalse();
    }
}

public class LogMicroserviceMutationTests
{
    private readonly Mock<ILogMicroserviceService> _mockService;
    private readonly LogMicroserviceMutation _mutation;

    public LogMicroserviceMutationTests()
    {
        _mockService = new Mock<ILogMicroserviceService>();
        _mutation = new LogMicroserviceMutation();
    }

    [Fact]
    public async Task CreateLogMicroservice_ShouldCreateAndReturnDto()
    {
        // Arrange
        var input = new CreateLogMicroserviceInput
        {
            LogId = 1,
            LogMicroserviceText = "Test microservice log"
        };

        var expectedDto = new LogMicroserviceDto
        {
            LogId = 1,
            LogMicroserviceText = "Test microservice log"
        };

        _mockService.Setup(s => s.CreateAsync(
            It.IsAny<CreateLogMicroserviceDto>(),
            null,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _mutation.CreateLogMicroservice(_mockService.Object, input);

        // Assert
        result.Should().NotBeNull();
        result.LogId.Should().Be(1);
        result.LogMicroserviceText.Should().Be("Test microservice log");
    }
}
