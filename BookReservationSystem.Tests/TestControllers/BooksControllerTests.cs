using BookReservationSystem.Application.Models;
using BookReservationSystem.Service.IService;
using BookReservationSystem.WebApi.Controllers;
using BookReservationSystem.WebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class BooksControllerTests
{
    private readonly BooksController _controller;
    private readonly Mock<IBookingService> _mockBookingService; // Mock of the service
    private readonly ILogger<BooksController> _logger;

    public BooksControllerTests()
    {
        _mockBookingService = new Mock<IBookingService>();
        _logger = new Logger<BooksController>(new LoggerFactory()); // Set up a logger instance

        _controller = new BooksController(_mockBookingService.Object, _logger); // Initialize your controller with the mock service
    }

    [Fact]
    public async Task GetBooks_ReturnsListOfBooks()
    {
        // Arrange
        var books = new List<BookDto>
        {
            new BookDto { Id = 1, Title = "Test Book 1", Author = "Author 1", IsReserved = false },
            new BookDto { Id = 2, Title = "Test Book 2", Author = "Author 2", IsReserved = true },
        };

        // Mock the service method
        _mockBookingService.Setup(service => service.GetAllBooksAsync())
            .ReturnsAsync(books);

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<List<BookDto>>>(okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Books retrieved successfully.", response.Message);
        Assert.Equal(books.Count, response.Data.Count);
    }

    [Fact]
    public async Task GetBook_ReturnsBook_WhenBookExists()
    {
        // Arrange
        var book = new BookDto { Id = 1, Title = "Test Book", Author = "Author" };
        _mockBookingService.Setup(service => service.GetBookByIdAsync(1))
            .ReturnsAsync(book);

        // Act
        var result = await _controller.GetBook(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<BookDto>>(okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Book retrieved successfully.", response.Message);
        Assert.Equal(book.Title, response.Data.Title);
        Assert.Equal(book.Author, response.Data.Author);
    }

    [Fact]
    public async Task AddBook_ValidBook_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newBook = new BookDto { Title = "New Book", Author = "New Author" };
        _mockBookingService.Setup(service => service.AddBookAsync(newBook))
            .ReturnsAsync(newBook);

        // Act
        var result = await _controller.AddBook(newBook);

        // Assert
        var createdResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<BookDto>>(createdResult.Value);
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        Assert.Equal("Book created successfully.", response.Message);
        Assert.Equal(newBook.Title, response.Data.Title);
        Assert.Equal(newBook.Author, response.Data.Author);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var existingBook = new BookDto { Id = 1, Title = "Old Title", Author = "Old Author" };
        var updatedBook = new BookDto { Id = 1, Title = "Updated Title", Author = "Updated Author" };

        _mockBookingService.Setup(service => service.UpdateBookAsync(updatedBook))
            .ReturnsAsync(updatedBook);

        // Act
        var result = await _controller.UpdateBook(1, updatedBook);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContent_WhenBookExists()
    {
        // Arrange
        var bookId = 1;
        _mockBookingService.Setup(service => service.DeleteBookAsync(bookId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteBook(bookId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ReserveBook_ReturnsNoContent_WhenReservationIsSuccessful()
    {
        // Arrange
        var bookId = 1;
        var comment = "Reserve Comment";
        _mockBookingService.Setup(service => service.ReserveBookAsync(bookId, comment))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.ReserveBook(bookId, comment);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UnreserveBook_ReturnsNoContent_WhenUnreservationIsSuccessful()
    {
        // Arrange
        var bookId = 1;
        _mockBookingService.Setup(service => service.UnreserveBookAsync(bookId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UnreserveBook(bookId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetReservedBooks_ReturnsListOfReservedBooks()
    {
        // Arrange
        var reservedBooks = new List<BookDto>
            {
                new BookDto { Id = 1, Title = "Reserved Book 1", Author = "Author 1", IsReserved = true },
            };

        _mockBookingService.Setup(service => service.GetReservedBooksAsync())
            .ReturnsAsync(reservedBooks);

        // Act
        var result = await _controller.GetReservedBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<List<BookDto>>>(okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Reserved books retrieved successfully.", response.Message);
        Assert.Single(response.Data);
    }

    [Fact]
    public async Task GetAvailableBooks_ReturnsListOfAvailableBooks()
    {
        // Arrange
        var availableBooks = new List<BookDto>
            {
                new BookDto { Id = 1, Title = "Available Book 1", Author = "Author 1", IsReserved = false },
            };

        _mockBookingService.Setup(service => service.GetAvailableBooksAsync())
            .ReturnsAsync(availableBooks);

        // Act
        var result = await _controller.GetAvailableBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<List<BookDto>>>(okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Available books retrieved successfully.", response.Message);
        Assert.Single(response.Data);
    }

    [Fact]
    public async Task GetStatusHistory_ReturnsListOfStatusHistories_WhenHistoriesExist()
    {
        // Arrange
        var bookId = 1;
        var histories = new List<BooksStatusHistoryDto>
            {
                new BooksStatusHistoryDto { Id = 1, BookId = bookId, IsReserved = true, Comment = "Reserved", ChangedAt = DateTime.UtcNow },
            };

        _mockBookingService.Setup(service => service.GetBookStatusHistoryAsync(bookId))
            .ReturnsAsync(histories);

        // Act
        var result = await _controller.GetStatusHistory(bookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<List<BooksStatusHistoryDto>>>(okResult.Value);
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("Status history retrieved successfully.", response.Message);
        Assert.Single(response.Data);
    }
}
