using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.Mappings;
using FastServer.Application.Services;
using FastServer.Domain;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace FastServer.Tests.Application.Services;

public class LogServicesHeaderServiceTests
{
    private readonly Mock<IDataSourceFactory> _mockFactory;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogServicesHeaderRepository> _mockRepository;
    private readonly IMapper _mapper;
    private readonly LogServicesHeaderService _service;

    public LogServicesHeaderServiceTests()
    {
        _mockFactory = new Mock<IDataSourceFactory>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRepository = new Mock<ILogServicesHeaderRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        _mockUnitOfWork.Setup(u => u.LogServicesHeaders).Returns(_mockRepository.Object);
        _mockFactory.Setup(f => f.CreateUnitOfWork(It.IsAny<DataSourceType>())).Returns(_mockUnitOfWork.Object);

        var dataSourceSettings = new DataSourceSettings(DataSourceType.PostgreSQL);
        _service = new LogServicesHeaderService(_mockFactory.Object, _mapper, dataSourceSettings);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDto_WhenEntityExists()
    {
        // Arrange
        var entity = new LogServicesHeader
        {
            LogId = 1,
            LogDateIn = DateTime.UtcNow,
            LogDateOut = DateTime.UtcNow.AddSeconds(5),
            LogState = LogState.Completed,
            LogMethodUrl = "/api/test"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.LogId.Should().Be(1);
        result.LogMethodUrl.Should().Be("/api/test");
        result.LogState.Should().Be(LogState.Completed);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LogServicesHeader?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedDto()
    {
        // Arrange
        var createDto = new CreateLogServicesHeaderDto
        {
            LogDateIn = DateTime.UtcNow,
            LogDateOut = DateTime.UtcNow.AddSeconds(5),
            LogState = LogState.Completed,
            LogMethodUrl = "/api/test"
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<LogServicesHeader>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LogServicesHeader entity, CancellationToken _) =>
            {
                entity.LogId = 1;
                return entity;
            });

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.LogMethodUrl.Should().Be("/api/test");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<LogServicesHeader>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenEntityExists()
    {
        // Arrange
        var entity = new LogServicesHeader { LogId = 1 };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        _mockRepository.Setup(r => r.DeleteAsync(entity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenEntityNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((LogServicesHeader?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<LogServicesHeader>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedResult()
    {
        // Arrange
        var entities = new List<LogServicesHeader>
        {
            new() { LogId = 1, LogMethodUrl = "/api/test1" },
            new() { LogId = 2, LogMethodUrl = "/api/test2" },
            new() { LogId = 3, LogMethodUrl = "/api/test3" }
        };

        _mockRepository.Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(entities);

        var pagination = new PaginationParamsDto { PageNumber = 1, PageSize = 2 };

        // Act
        var result = await _service.GetAllAsync(pagination);

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(3);
        result.Items.Should().HaveCount(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.TotalPages.Should().Be(2);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }
}
