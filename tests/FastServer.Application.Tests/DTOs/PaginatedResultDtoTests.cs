using FastServer.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace FastServer.Application.Tests.DTOs;

public class PaginatedResultDtoTests
{
    [Fact]
    public void PaginatedResultDto_ShouldCalculateTotalPages_Correctly()
    {
        // Arrange
        var result = new PaginatedResultDto<string>
        {
            Items = new[] { "item1", "item2" },
            TotalCount = 10,
            PageNumber = 1,
            PageSize = 3
        };

        // Assert
        result.TotalPages.Should().Be(4); // ceil(10/3) = 4
    }

    [Fact]
    public void PaginatedResultDto_ShouldIndicateHasNextPage_WhenNotOnLastPage()
    {
        // Arrange
        var result = new PaginatedResultDto<string>
        {
            Items = new[] { "item1" },
            TotalCount = 10,
            PageNumber = 1,
            PageSize = 5
        };

        // Assert
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void PaginatedResultDto_ShouldIndicateHasPreviousPage_WhenNotOnFirstPage()
    {
        // Arrange
        var result = new PaginatedResultDto<string>
        {
            Items = new[] { "item1" },
            TotalCount = 10,
            PageNumber = 2,
            PageSize = 5
        };

        // Assert
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void PaginatedResultDto_ShouldIndicateNoPagination_WhenOnlyOnePage()
    {
        // Arrange
        var result = new PaginatedResultDto<string>
        {
            Items = new[] { "item1", "item2" },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        // Assert
        result.TotalPages.Should().Be(1);
        result.HasNextPage.Should().BeFalse();
        result.HasPreviousPage.Should().BeFalse();
    }
}

public class PaginationParamsDtoTests
{
    [Fact]
    public void PaginationParamsDto_ShouldHaveDefaultValues()
    {
        // Arrange
        var pagination = new PaginationParamsDto();

        // Assert
        pagination.PageNumber.Should().Be(1);
        pagination.PageSize.Should().Be(10);
    }

    [Fact]
    public void PaginationParamsDto_ShouldCalculateSkip_Correctly()
    {
        // Arrange
        var pagination = new PaginationParamsDto { PageNumber = 3, PageSize = 10 };

        // Assert
        pagination.Skip.Should().Be(20); // (3-1) * 10 = 20
    }

    [Theory]
    [InlineData(1, 10, 0)]
    [InlineData(2, 10, 10)]
    [InlineData(5, 20, 80)]
    [InlineData(1, 50, 0)]
    public void PaginationParamsDto_ShouldCalculateSkip_ForVariousPages(int pageNumber, int pageSize, int expectedSkip)
    {
        // Arrange
        var pagination = new PaginationParamsDto { PageNumber = pageNumber, PageSize = pageSize };

        // Assert
        pagination.Skip.Should().Be(expectedSkip);
    }
}
