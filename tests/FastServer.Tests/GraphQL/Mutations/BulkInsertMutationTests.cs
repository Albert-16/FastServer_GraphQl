using AutoMapper;
using FastServer.Application.DTOs;
using FastServer.Application.EventPublishers;
using FastServer.Application.Events.LogEvents;
using FastServer.Application.Events.LogMicroserviceEvents;
using FastServer.Application.Interfaces;
using FastServer.Application.Mappings;
using FastServer.Application.Services;
using FastServer.Domain.Entities;
using FastServer.Domain.Enums;
using FastServer.GraphQL.Api.GraphQL.Mutations;
using FastServer.GraphQL.Api.GraphQL.Types;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Xunit;

namespace FastServer.Tests.GraphQL.Mutations;

#region Test Infrastructure

/// <summary>
/// DbContext in-memory para tests que implementa ILogsDbContext
/// </summary>
public class TestLogsDbContext : DbContext, ILogsDbContext
{
    public TestLogsDbContext(DbContextOptions<TestLogsDbContext> options) : base(options) { }

    public DbSet<LogServicesHeader> LogServicesHeaders => Set<LogServicesHeader>();
    public DbSet<LogMicroservice> LogMicroservices => Set<LogMicroservice>();
    public DbSet<LogServicesContent> LogServicesContents => Set<LogServicesContent>();
    public DbSet<LogServicesHeaderHistorico> LogServicesHeadersHistorico => Set<LogServicesHeaderHistorico>();
    public DbSet<LogMicroserviceHistorico> LogMicroservicesHistorico => Set<LogMicroserviceHistorico>();
    public DbSet<LogServicesContentHistorico> LogServicesContentsHistorico => Set<LogServicesContentHistorico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LogServicesHeader>(b =>
        {
            b.HasKey(e => e.LogId);
            b.Property(e => e.LogId).ValueGeneratedOnAdd();
            b.Property(e => e.LogState).HasConversion<string>();
        });

        modelBuilder.Entity<LogMicroservice>(b =>
        {
            b.HasKey(e => e.LogId);
        });

        modelBuilder.Entity<LogServicesContent>(b =>
        {
            b.HasKey(e => e.LogId);
        });

        modelBuilder.Entity<LogServicesHeaderHistorico>(b =>
        {
            b.HasKey(e => e.LogId);
        });

        modelBuilder.Entity<LogMicroserviceHistorico>(b =>
        {
            b.HasKey(e => e.LogId);
        });

        modelBuilder.Entity<LogServicesContentHistorico>(b =>
        {
            b.HasKey(e => e.LogId);
        });
    }
}

#endregion

#region Mutation-Level Tests (Mock Service)

public class BulkCreateLogServicesHeaderMutationTests
{
    private readonly Mock<ILogServicesHeaderService> _mockService;
    private readonly LogServicesMutation _mutation;

    public BulkCreateLogServicesHeaderMutationTests()
    {
        _mockService = new Mock<ILogServicesHeaderService>();
        _mutation = new LogServicesMutation();
    }

    [Fact]
    public async Task BulkCreateLogServicesHeader_ShouldCallServiceAndReturnResult()
    {
        // Arrange
        var input = new BulkCreateLogServicesHeaderInput
        {
            Items = new List<CreateLogServicesHeaderInput>
            {
                new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/test1", HttpMethod = "GET" },
                new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(2), LogState = LogState.Completed, LogMethodUrl = "/api/test2", HttpMethod = "POST" },
                new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(3), LogState = LogState.Failed, LogMethodUrl = "/api/test3", HttpMethod = "PUT" }
            }
        };

        var expectedResult = new BulkInsertResultDto<LogServicesHeaderDto>
        {
            InsertedItems = new List<LogServicesHeaderDto>
            {
                new() { LogId = 1, LogMethodUrl = "/api/test1" },
                new() { LogId = 2, LogMethodUrl = "/api/test2" },
                new() { LogId = 3, LogMethodUrl = "/api/test3" }
            },
            TotalRequested = 3,
            TotalInserted = 3,
            TotalFailed = 0,
            Success = true
        };

        _mockService.Setup(s => s.CreateBulkAsync(
            It.IsAny<IEnumerable<CreateLogServicesHeaderDto>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _mutation.BulkCreateLogServicesHeader(_mockService.Object, input);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(3);
        result.TotalInserted.Should().Be(3);
        result.TotalFailed.Should().Be(0);
        result.InsertedItems.Should().HaveCount(3);
        _mockService.Verify(s => s.CreateBulkAsync(
            It.Is<IEnumerable<CreateLogServicesHeaderDto>>(d => d.Count() == 3),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task BulkCreateLogServicesHeader_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var input = new BulkCreateLogServicesHeaderInput
        {
            Items = new List<CreateLogServicesHeaderInput>
            {
                new()
                {
                    LogDateIn = now,
                    LogDateOut = now.AddSeconds(5),
                    LogState = LogState.Completed,
                    LogMethodUrl = "/api/users",
                    LogMethodName = "GetUsers",
                    LogFsId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                    MethodDescription = "Obtiene usuarios",
                    TciIpPort = "192.168.1.1:8080",
                    IpFs = "10.0.0.1",
                    TypeProcess = "REST",
                    LogNodo = "node-1",
                    HttpMethod = "GET",
                    MicroserviceName = "user-service",
                    RequestDuration = 150,
                    TransactionId = "txn-001",
                    UserId = "user-123",
                    SessionId = "sess-456",
                    RequestId = 789
                }
            }
        };

        CreateLogServicesHeaderDto? capturedDto = null;
        _mockService.Setup(s => s.CreateBulkAsync(
            It.IsAny<IEnumerable<CreateLogServicesHeaderDto>>(),
            It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<CreateLogServicesHeaderDto>, CancellationToken>((dtos, _) =>
                capturedDto = dtos.First())
            .ReturnsAsync(new BulkInsertResultDto<LogServicesHeaderDto> { Success = true, TotalRequested = 1, TotalInserted = 1 });

        // Act
        await _mutation.BulkCreateLogServicesHeader(_mockService.Object, input);

        // Assert
        capturedDto.Should().NotBeNull();
        capturedDto!.LogDateIn.Should().Be(now);
        capturedDto.LogDateOut.Should().Be(now.AddSeconds(5));
        capturedDto.LogState.Should().Be(LogState.Completed);
        capturedDto.LogMethodUrl.Should().Be("/api/users");
        capturedDto.LogMethodName.Should().Be("GetUsers");
        capturedDto.LogFsId.Should().Be(Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"));
        capturedDto.MicroserviceName.Should().Be("user-service");
        capturedDto.HttpMethod.Should().Be("GET");
        capturedDto.TransactionId.Should().Be("txn-001");
        capturedDto.UserId.Should().Be("user-123");
        capturedDto.SessionId.Should().Be("sess-456");
        capturedDto.RequestId.Should().Be(789);
        capturedDto.RequestDuration.Should().Be(150);
    }
}

public class BulkCreateLogMicroserviceMutationTests
{
    private readonly Mock<ILogMicroserviceService> _mockService;
    private readonly LogMicroserviceMutation _mutation;

    public BulkCreateLogMicroserviceMutationTests()
    {
        _mockService = new Mock<ILogMicroserviceService>();
        _mutation = new LogMicroserviceMutation();
    }

    [Fact]
    public async Task BulkCreateLogMicroservice_ShouldCallServiceAndReturnResult()
    {
        // Arrange
        var input = new BulkCreateLogMicroserviceInput
        {
            Items = new List<CreateLogMicroserviceInput>
            {
                new() { LogId = 100, RequestId = 1, EventName = "TestEvent1", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = "Log 1" },
                new() { LogId = 101, RequestId = 2, EventName = "TestEvent2", LogDate = DateTime.UtcNow, LogLevel = "WARN", LogMicroserviceText = "Log 2" }
            }
        };

        var expectedResult = new BulkInsertResultDto<LogMicroserviceDto>
        {
            InsertedItems = new List<LogMicroserviceDto>
            {
                new() { LogId = 100, LogLevel = "INFO", LogMicroserviceText = "Log 1" },
                new() { LogId = 101, LogLevel = "WARN", LogMicroserviceText = "Log 2" }
            },
            TotalRequested = 2,
            TotalInserted = 2,
            Success = true
        };

        _mockService.Setup(s => s.CreateBulkAsync(
            It.IsAny<IEnumerable<CreateLogMicroserviceDto>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _mutation.BulkCreateLogMicroservice(_mockService.Object, input);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(2);
        result.TotalInserted.Should().Be(2);
        result.InsertedItems.Should().HaveCount(2);
    }

    [Fact]
    public async Task BulkCreateLogMicroservice_ShouldMapAllFieldsCorrectly()
    {
        // Arrange
        var logDate = DateTime.UtcNow;
        var input = new BulkCreateLogMicroserviceInput
        {
            Items = new List<CreateLogMicroserviceInput>
            {
                new() { LogId = 55, RequestId = 1, EventName = "FailEvent", LogDate = logDate, LogLevel = "ERROR", LogMicroserviceText = "Something failed" }
            }
        };

        CreateLogMicroserviceDto? capturedDto = null;
        _mockService.Setup(s => s.CreateBulkAsync(
            It.IsAny<IEnumerable<CreateLogMicroserviceDto>>(),
            It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<CreateLogMicroserviceDto>, CancellationToken>((dtos, _) =>
                capturedDto = dtos.First())
            .ReturnsAsync(new BulkInsertResultDto<LogMicroserviceDto> { Success = true, TotalRequested = 1, TotalInserted = 1 });

        // Act
        await _mutation.BulkCreateLogMicroservice(_mockService.Object, input);

        // Assert
        capturedDto.Should().NotBeNull();
        capturedDto!.LogId.Should().Be(55);
        capturedDto.LogDate.Should().Be(logDate);
        capturedDto.LogLevel.Should().Be("ERROR");
        capturedDto.LogMicroserviceText.Should().Be("Something failed");
    }
}

#endregion

#region Service-Level Tests: LogServicesHeaderService

public class BulkCreateLogServicesHeaderServiceTests : IDisposable
{
    private readonly TestLogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly Mock<ILogEventPublisher> _mockEventPublisher;
    private readonly LogServicesHeaderService _service;

    public BulkCreateLogServicesHeaderServiceTests()
    {
        var options = new DbContextOptionsBuilder<TestLogsDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_Header_{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new TestLogsDbContext(options);

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();

        _mockEventPublisher = new Mock<ILogEventPublisher>();
        _mockEventPublisher.Setup(p => p.PublishLogCreatedAsync(It.IsAny<LogCreatedEvent>()))
            .Returns(Task.CompletedTask);

        _service = new LogServicesHeaderService(_context, _mapper, _mockEventPublisher.Object);
    }

    [Fact]
    public async Task CreateBulkAsync_WithValidData_ShouldInsertAllRecords()
    {
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/test1" },
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(2), LogState = LogState.Failed, LogMethodUrl = "/api/test2" },
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(3), LogState = LogState.Completed, LogMethodUrl = "/api/test3" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(3);
        result.TotalInserted.Should().Be(3);
        result.TotalFailed.Should().Be(0);
        result.Errors.Should().BeEmpty();

        var dbCount = await _context.LogServicesHeaders.CountAsync();
        dbCount.Should().Be(3);
    }

    [Fact]
    public async Task CreateBulkAsync_WithValidData_ShouldPreserveFieldValues()
    {
        var now = DateTime.UtcNow;
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new()
            {
                LogDateIn = now, LogDateOut = now.AddSeconds(5), LogState = LogState.Completed,
                LogMethodUrl = "/api/users/list", LogMethodName = "ListUsers", LogFsId = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                MicroserviceName = "user-management", HttpMethod = "GET",
                TransactionId = "txn-abc-123", UserId = "admin-001", RequestDuration = 250, RequestId = 12345
            }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        var inserted = result.InsertedItems.First();
        inserted.LogMethodUrl.Should().Be("/api/users/list");
        inserted.MicroserviceName.Should().Be("user-management");
        inserted.TransactionId.Should().Be("txn-abc-123");
        inserted.RequestDuration.Should().Be(250);

        var dbEntity = await _context.LogServicesHeaders.FirstAsync();
        dbEntity.LogMethodUrl.Should().Be("/api/users/list");
    }

    [Fact]
    public async Task CreateBulkAsync_WithEmptyList_ShouldReturnSuccessWithZeroItems()
    {
        var result = await _service.CreateBulkAsync(new List<CreateLogServicesHeaderDto>());

        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(0);
        result.TotalInserted.Should().Be(0);
    }

    [Fact]
    public async Task CreateBulkAsync_ExceedingLimit_ShouldReturnFailure()
    {
        var dtos = Enumerable.Range(1, 1001).Select(i => new CreateLogServicesHeaderDto
        {
            LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(i),
            LogState = LogState.Completed, LogMethodUrl = $"/api/test{i}"
        }).ToList();

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeFalse();
        result.TotalRequested.Should().Be(1001);
        result.TotalInserted.Should().Be(0);
        result.ErrorMessage.Should().Contain("1000");

        var dbCount = await _context.LogServicesHeaders.CountAsync();
        dbCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateBulkAsync_WithExactly1000Items_ShouldSucceed()
    {
        var dtos = Enumerable.Range(1, 1000).Select(i => new CreateLogServicesHeaderDto
        {
            LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(i),
            LogState = LogState.Completed, LogMethodUrl = $"/api/test{i}"
        }).ToList();

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        result.TotalInserted.Should().Be(1000);
    }

    // ── Partial Success Tests ──

    [Fact]
    public async Task CreateBulkAsync_WithMixedValidAndInvalid_ShouldInsertOnlyValid()
    {
        // 2 válidos + 1 inválido (LogMethodUrl vacío)
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/valid1" },
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(2), LogState = LogState.Completed, LogMethodUrl = "" },  // inválido
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(3), LogState = LogState.Completed, LogMethodUrl = "/api/valid2" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(3);
        result.TotalInserted.Should().Be(2);
        result.TotalFailed.Should().Be(1);
        result.InsertedItems.Should().HaveCount(2);
        result.Errors.Should().HaveCount(1);
        result.Errors.First().Index.Should().Be(1); // índice del item inválido

        var dbCount = await _context.LogServicesHeaders.CountAsync();
        dbCount.Should().Be(2);
    }

    [Fact]
    public async Task CreateBulkAsync_WithInvalidDateRange_ShouldRejectItem()
    {
        // LogDateOut anterior a LogDateIn
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/valid" },
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(-10), LogState = LogState.Completed, LogMethodUrl = "/api/bad-dates" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        result.TotalInserted.Should().Be(1);
        result.TotalFailed.Should().Be(1);
        result.Errors.First().ErrorMessage.Should().Contain("LogDateOut");
    }

    [Fact]
    public async Task CreateBulkAsync_WithDefaultDateIn_ShouldRejectItem()
    {
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new() { LogDateIn = default, LogDateOut = DateTime.UtcNow, LogState = LogState.Completed, LogMethodUrl = "/api/test" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeFalse();
        result.TotalInserted.Should().Be(0);
        result.TotalFailed.Should().Be(1);
        result.Errors.First().ErrorMessage.Should().Contain("LogDateIn");
    }

    [Fact]
    public async Task CreateBulkAsync_AllInvalid_ShouldReturnFailureWithAllErrors()
    {
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "" },
            new() { LogDateIn = default, LogDateOut = DateTime.UtcNow, LogState = LogState.Completed, LogMethodUrl = "/api/test" },
            new() { LogDateIn = DateTime.UtcNow, LogDateOut = DateTime.UtcNow.AddSeconds(-5), LogState = LogState.Completed, LogMethodUrl = "/api/bad" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeFalse();
        result.TotalRequested.Should().Be(3);
        result.TotalInserted.Should().Be(0);
        result.TotalFailed.Should().Be(3);
        result.Errors.Should().HaveCount(3);
        result.ErrorMessage.Should().Contain("Ningún registro");

        var dbCount = await _context.LogServicesHeaders.CountAsync();
        dbCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateBulkAsync_ErrorsPreserveOriginalIndex()
    {
        // Items en posiciones 0,1,2,3,4 - invalidos en 1 y 3
        var now = DateTime.UtcNow;
        var dtos = new List<CreateLogServicesHeaderDto>
        {
            new() { LogDateIn = now, LogDateOut = now.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/ok0" },
            new() { LogDateIn = now, LogDateOut = now.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "" },          // idx 1 inválido
            new() { LogDateIn = now, LogDateOut = now.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/ok2" },
            new() { LogDateIn = now, LogDateOut = now.AddSeconds(-1), LogState = LogState.Completed, LogMethodUrl = "/api/bad3" }, // idx 3 inválido
            new() { LogDateIn = now, LogDateOut = now.AddSeconds(1), LogState = LogState.Completed, LogMethodUrl = "/api/ok4" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.TotalInserted.Should().Be(3);
        result.TotalFailed.Should().Be(2);
        result.Errors.Select(e => e.Index).Should().BeEquivalentTo(new[] { 1, 3 });
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

#endregion

#region Mutation-Level Tests: UpdateLogServicesHeader (new fields mapping)

public class UpdateLogServicesHeaderMutationTests
{
    private readonly Mock<ILogServicesHeaderService> _mockService;
    private readonly LogServicesMutation _mutation;

    public UpdateLogServicesHeaderMutationTests()
    {
        _mockService = new Mock<ILogServicesHeaderService>();
        _mutation = new LogServicesMutation();
    }

    [Fact]
    public async Task UpdateLogServicesHeader_ShouldMapNewFieldsToDto()
    {
        // Arrange
        var input = new UpdateLogServicesHeaderInput
        {
            LogId = 42,
            LogMethodName = "UpdatedMethod",
            MethodDescription = "Updated description",
            TciIpPort = "10.0.0.5:9090",
            IpFs = "192.168.0.10",
            TypeProcess = "ASYNC",
            LogNodo = "node-updated",
            MicroserviceName = "updated-service",
            UserId = "user-updated"
        };

        UpdateLogServicesHeaderDto? capturedDto = null;
        _mockService.Setup(s => s.UpdateAsync(It.IsAny<UpdateLogServicesHeaderDto>(), It.IsAny<CancellationToken>()))
            .Callback<UpdateLogServicesHeaderDto, CancellationToken>((dto, _) => capturedDto = dto)
            .ReturnsAsync(new LogServicesHeaderDto { LogId = 42 });

        // Act
        await _mutation.UpdateLogServicesHeader(_mockService.Object, input);

        // Assert
        capturedDto.Should().NotBeNull();
        capturedDto!.LogId.Should().Be(42);
        capturedDto.LogMethodName.Should().Be("UpdatedMethod");
        capturedDto.MethodDescription.Should().Be("Updated description");
        capturedDto.TciIpPort.Should().Be("10.0.0.5:9090");
        capturedDto.IpFs.Should().Be("192.168.0.10");
        capturedDto.TypeProcess.Should().Be("ASYNC");
        capturedDto.LogNodo.Should().Be("node-updated");
        capturedDto.MicroserviceName.Should().Be("updated-service");
        capturedDto.UserId.Should().Be("user-updated");
    }

    [Fact]
    public async Task BulkUpdateLogServicesHeader_ShouldMapNewFieldsToDto()
    {
        // Arrange
        var input = new BulkUpdateLogServicesHeaderInput
        {
            Items = new List<UpdateLogServicesHeaderInput>
            {
                new() { LogId = 1, MicroserviceName = "svc-A", UserId = "usr-1" },
                new() { LogId = 2, LogMethodName = "GetData", TypeProcess = "REST" }
            }
        };

        IEnumerable<UpdateLogServicesHeaderDto>? capturedDtos = null;
        _mockService.Setup(s => s.UpdateBulkAsync(It.IsAny<IEnumerable<UpdateLogServicesHeaderDto>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<UpdateLogServicesHeaderDto>, CancellationToken>((dtos, _) => capturedDtos = dtos.ToList())
            .ReturnsAsync(new BulkUpdateResultDto<LogServicesHeaderDto> { Success = true, TotalRequested = 2, TotalUpdated = 2 });

        // Act
        await _mutation.BulkUpdateLogServicesHeader(_mockService.Object, input);

        // Assert
        capturedDtos.Should().NotBeNull();
        var dtoList = capturedDtos!.ToList();
        dtoList.Should().HaveCount(2);

        dtoList[0].LogId.Should().Be(1);
        dtoList[0].MicroserviceName.Should().Be("svc-A");
        dtoList[0].UserId.Should().Be("usr-1");

        dtoList[1].LogId.Should().Be(2);
        dtoList[1].LogMethodName.Should().Be("GetData");
        dtoList[1].TypeProcess.Should().Be("REST");
    }
}

#endregion

#region Service-Level Tests: UpdateLogServicesHeaderService (new fields + partial update)

public class UpdateLogServicesHeaderServiceTests : IDisposable
{
    private readonly TestLogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly Mock<ILogEventPublisher> _mockEventPublisher;
    private readonly LogServicesHeaderService _service;

    public UpdateLogServicesHeaderServiceTests()
    {
        var options = new DbContextOptionsBuilder<TestLogsDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_Update_{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new TestLogsDbContext(options);

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();

        _mockEventPublisher = new Mock<ILogEventPublisher>();
        _mockEventPublisher.Setup(p => p.PublishLogUpdatedAsync(It.IsAny<LogUpdatedEvent>()))
            .Returns(Task.CompletedTask);

        _service = new LogServicesHeaderService(_context, _mapper, _mockEventPublisher.Object);
    }

    private async Task<LogServicesHeader> SeedEntityAsync()
    {
        var entity = new LogServicesHeader
        {
            LogDateIn = DateTime.UtcNow,
            LogDateOut = DateTime.UtcNow.AddSeconds(1),
            LogState = LogState.Completed,
            LogMethodUrl = "/api/original",
            LogMethodName = "OriginalMethod",
            MethodDescription = "Original description",
            TciIpPort = "1.1.1.1:80",
            IpFs = "2.2.2.2",
            TypeProcess = "SYNC",
            LogNodo = "node-original",
            MicroserviceName = "original-service",
            UserId = "original-user",
            ErrorCode = null
        };
        await _context.LogServicesHeaders.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNewFields()
    {
        // Arrange
        var entity = await SeedEntityAsync();
        var dto = new UpdateLogServicesHeaderDto
        {
            LogId = entity.LogId,
            LogMethodName = "NewMethod",
            MethodDescription = "New description",
            TciIpPort = "9.9.9.9:443",
            IpFs = "8.8.8.8",
            TypeProcess = "ASYNC",
            LogNodo = "node-new",
            MicroserviceName = "new-service",
            UserId = "new-user"
        };

        // Act
        await _service.UpdateAsync(dto);

        // Assert
        var updated = await _context.LogServicesHeaders.FirstAsync(x => x.LogId == entity.LogId);
        updated.LogMethodName.Should().Be("NewMethod");
        updated.MethodDescription.Should().Be("New description");
        updated.TciIpPort.Should().Be("9.9.9.9:443");
        updated.IpFs.Should().Be("8.8.8.8");
        updated.TypeProcess.Should().Be("ASYNC");
        updated.LogNodo.Should().Be("node-new");
        updated.MicroserviceName.Should().Be("new-service");
        updated.UserId.Should().Be("new-user");
    }

    [Fact]
    public async Task UpdateAsync_NullFields_ShouldNotOverwriteExistingValues()
    {
        // Arrange — sembrar entidad con valores en los nuevos campos
        var entity = await SeedEntityAsync();
        var dto = new UpdateLogServicesHeaderDto
        {
            LogId = entity.LogId,
            // Todos los nuevos campos se dejan null — no deben sobreescribir
            ErrorCode = "E500"
        };

        // Act
        await _service.UpdateAsync(dto);

        // Assert — los nuevos campos mantienen sus valores originales
        var updated = await _context.LogServicesHeaders.FirstAsync(x => x.LogId == entity.LogId);
        updated.LogMethodName.Should().Be("OriginalMethod");
        updated.MethodDescription.Should().Be("Original description");
        updated.TciIpPort.Should().Be("1.1.1.1:80");
        updated.IpFs.Should().Be("2.2.2.2");
        updated.TypeProcess.Should().Be("SYNC");
        updated.LogNodo.Should().Be("node-original");
        updated.MicroserviceName.Should().Be("original-service");
        updated.UserId.Should().Be("original-user");
        updated.ErrorCode.Should().Be("E500"); // este sí se actualizó
    }

    [Fact]
    public async Task UpdateBulkAsync_ShouldUpdateNewFieldsOnMultipleEntities()
    {
        // Arrange
        var e1 = await SeedEntityAsync();
        var e2 = await SeedEntityAsync();

        var dtos = new List<UpdateLogServicesHeaderDto>
        {
            new() { LogId = e1.LogId, MicroserviceName = "svc-updated-1", UserId = "user-updated-1" },
            new() { LogId = e2.LogId, LogMethodName = "BulkMethod", TypeProcess = "BATCH" }
        };

        // Act
        var result = await _service.UpdateBulkAsync(dtos);

        // Assert
        result.Success.Should().BeTrue();
        result.TotalUpdated.Should().Be(2);

        var updated1 = await _context.LogServicesHeaders.FirstAsync(x => x.LogId == e1.LogId);
        updated1.MicroserviceName.Should().Be("svc-updated-1");
        updated1.UserId.Should().Be("user-updated-1");
        updated1.LogMethodName.Should().Be("OriginalMethod"); // no se tocó

        var updated2 = await _context.LogServicesHeaders.FirstAsync(x => x.LogId == e2.LogId);
        updated2.LogMethodName.Should().Be("BulkMethod");
        updated2.TypeProcess.Should().Be("BATCH");
        updated2.MicroserviceName.Should().Be("original-service"); // no se tocó
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

#endregion
#region Service-Level Tests: LogMicroserviceService

public class BulkCreateLogMicroserviceServiceTests : IDisposable
{
    private readonly TestLogsDbContext _context;
    private readonly IMapper _mapper;
    private readonly Mock<ILogMicroserviceEventPublisher> _mockEventPublisher;
    private readonly LogMicroserviceService _service;

    public BulkCreateLogMicroserviceServiceTests()
    {
        var options = new DbContextOptionsBuilder<TestLogsDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_Micro_{Guid.NewGuid()}")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new TestLogsDbContext(options);

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();

        _mockEventPublisher = new Mock<ILogMicroserviceEventPublisher>();
        _mockEventPublisher.Setup(p => p.PublishLogMicroserviceCreatedAsync(It.IsAny<LogMicroserviceCreatedEvent>()))
            .Returns(Task.CompletedTask);

        _service = new LogMicroserviceService(_context, _mapper, _mockEventPublisher.Object);
    }

    [Fact]
    public async Task CreateBulkAsync_WithValidData_ShouldInsertAllRecords()
    {
        var dtos = new List<CreateLogMicroserviceDto>
        {
            new() { LogId = 1001, RequestId = 1, EventName = "TestEvent1", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = "Starting process" },
            new() { LogId = 1002, RequestId = 2, EventName = "TestEvent2", LogDate = DateTime.UtcNow, LogLevel = "WARN", LogMicroserviceText = "Slow response" },
            new() { LogId = 1003, RequestId = 3, EventName = "TestEvent3", LogDate = DateTime.UtcNow, LogLevel = "ERROR", LogMicroserviceText = "Timeout" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(3);
        result.TotalInserted.Should().Be(3);
        result.TotalFailed.Should().Be(0);
        result.Errors.Should().BeEmpty();

        var dbCount = await _context.LogMicroservices.CountAsync();
        dbCount.Should().Be(3);
    }

    [Fact]
    public async Task CreateBulkAsync_WithValidData_ShouldPreserveFieldValues()
    {
        var logDate = DateTime.UtcNow;
        var dtos = new List<CreateLogMicroserviceDto>
        {
            new() { LogId = 500, RequestId = 1, EventName = "ErrorEvent", LogDate = logDate, LogLevel = "ERROR", LogMicroserviceText = "NullReferenceException en UserController.Get" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        var inserted = result.InsertedItems.First();
        inserted.LogId.Should().Be(500);
        inserted.LogLevel.Should().Be("ERROR");

        var dbEntity = await _context.LogMicroservices.FirstOrDefaultAsync(x => x.LogId == 500);
        dbEntity.Should().NotBeNull();
        dbEntity!.LogMicroserviceText.Should().Be("NullReferenceException en UserController.Get");
    }

    [Fact]
    public async Task CreateBulkAsync_WithEmptyList_ShouldReturnSuccessWithZeroItems()
    {
        var result = await _service.CreateBulkAsync(new List<CreateLogMicroserviceDto>());

        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(0);
        result.TotalInserted.Should().Be(0);
    }

    [Fact]
    public async Task CreateBulkAsync_ExceedingLimit_ShouldReturnFailure()
    {
        var dtos = Enumerable.Range(1, 1001).Select(i => new CreateLogMicroserviceDto
        {
            LogId = i + 10000, RequestId = i, EventName = $"Event{i}", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = $"Log {i}"
        }).ToList();

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeFalse();
        result.TotalRequested.Should().Be(1001);
        result.ErrorMessage.Should().Contain("1000");

        var dbCount = await _context.LogMicroservices.CountAsync();
        dbCount.Should().Be(0);
    }

    // ── Partial Success Tests ──

    [Fact]
    public async Task CreateBulkAsync_WithInvalidLogId_ShouldRejectItem()
    {
        var dtos = new List<CreateLogMicroserviceDto>
        {
            new() { LogId = 100, RequestId = 1, EventName = "TestEvent", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = "Valid" },
            new() { LogId = 0, RequestId = 2, EventName = "TestEvent", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = "Invalid LogId" },   // inválido
            new() { LogId = -5, RequestId = 3, EventName = "TestEvent", LogDate = DateTime.UtcNow, LogLevel = "WARN", LogMicroserviceText = "Negative LogId" }, // inválido
            new() { LogId = 200, RequestId = 4, EventName = "TestEvent", LogDate = DateTime.UtcNow, LogLevel = "ERROR", LogMicroserviceText = "Valid too" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeTrue();
        result.TotalRequested.Should().Be(4);
        result.TotalInserted.Should().Be(2);
        result.TotalFailed.Should().Be(2);
        result.Errors.Should().HaveCount(2);
        result.Errors.Select(e => e.Index).Should().BeEquivalentTo(new[] { 1, 2 });

        var dbCount = await _context.LogMicroservices.CountAsync();
        dbCount.Should().Be(2);
    }

    [Fact]
    public async Task CreateBulkAsync_AllInvalid_ShouldReturnFailure()
    {
        var dtos = new List<CreateLogMicroserviceDto>
        {
            new() { LogId = 0, RequestId = 1, EventName = "TestEvent", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = "Bad 1" },
            new() { LogId = -1, RequestId = 2, EventName = "TestEvent", LogDate = DateTime.UtcNow, LogLevel = "INFO", LogMicroserviceText = "Bad 2" }
        };

        var result = await _service.CreateBulkAsync(dtos);

        result.Success.Should().BeFalse();
        result.TotalInserted.Should().Be(0);
        result.TotalFailed.Should().Be(2);
        result.ErrorMessage.Should().Contain("Ningún registro");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

#endregion
