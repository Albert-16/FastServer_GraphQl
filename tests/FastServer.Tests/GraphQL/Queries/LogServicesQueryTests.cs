using FastServer.Application.DTOs;
using FastServer.Application.Interfaces;
using FastServer.Domain.Enums;
using FastServer.GraphQL.Api.GraphQL.Queries;
using FastServer.GraphQL.Api.GraphQL.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastServer.Tests.GraphQL.Queries;

public class LogServicesQueryTests
{
    private readonly Mock<ILogServicesHeaderService> _mockService;
    private readonly LogServicesQuery _query;

    public LogServicesQueryTests()
    {
        _mockService = new Mock<ILogServicesHeaderService>();
        _query = new LogServicesQuery();
    }

    [Fact]
    public async Task GetLogById_ShouldReturnLog_WhenExists()
    {
        // Arrange
        var expectedLog = new LogServicesHeaderDto
        {
            LogId = 1,
            LogMethodUrl = "/api/test",
            LogState = LogState.Completed
        };

        _mockService.Setup(s => s.GetByIdAsync(1, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLog);

        // Act
        var result = await _query.GetLogById(_mockService.Object, 1);

        // Assert
        result.Should().NotBeNull();
        result!.LogId.Should().Be(1);
        result.LogMethodUrl.Should().Be("/api/test");
    }

    [Fact]
    public async Task GetLogById_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(999, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LogServicesHeaderDto?)null);

        // Act
        var result = await _query.GetLogById(_mockService.Object, 999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllLogs_ShouldReturnPaginatedResult()
    {
        // Arrange
        var expectedResult = new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = new List<LogServicesHeaderDto>
            {
                new() { LogId = 1, LogMethodUrl = "/api/test1" },
                new() { LogId = 2, LogMethodUrl = "/api/test2" }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _mockService.Setup(s => s.GetAllAsync(
            It.IsAny<PaginationParamsDto>(),
            null,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _query.GetAllLogs(_mockService.Object, null, null);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetLogsByFilter_ShouldApplyFilters()
    {
        // Arrange
        var filter = new LogFilterInput
        {
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow,
            State = LogState.Failed,
            MicroserviceName = "TestService"
        };

        var expectedResult = new PaginatedResultDto<LogServicesHeaderDto>
        {
            Items = new List<LogServicesHeaderDto>
            {
                new()
                {
                    LogId = 1,
                    LogMethodUrl = "/api/test",
                    LogState = LogState.Failed,
                    MicroserviceName = "TestService"
                }
            },
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 10
        };

        _mockService.Setup(s => s.GetByFilterAsync(
            It.Is<LogFilterDto>(f =>
                f.State == LogState.Failed &&
                f.MicroserviceName == "TestService"),
            It.IsAny<PaginationParamsDto>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _query.GetLogsByFilter(_mockService.Object, filter, null);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().LogState.Should().Be(LogState.Failed);
    }

    [Fact]
    public async Task GetFailedLogs_ShouldReturnOnlyFailedLogs()
    {
        // Arrange
        var expectedLogs = new List<LogServicesHeaderDto>
        {
            new() { LogId = 1, LogState = LogState.Failed, ErrorCode = "ERR001" },
            new() { LogId = 2, LogState = LogState.Failed, ErrorCode = "ERR002" }
        };

        _mockService.Setup(s => s.GetFailedLogsAsync(null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLogs);

        // Act
        var result = await _query.GetFailedLogs(_mockService.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.All(l => l.LogState == LogState.Failed).Should().BeTrue();
    }
}
