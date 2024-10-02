using BookReservationSystem.Application.Models;
using BookReservationSystem.Service.IService;
using BookReservationSystem.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookReservationSystem.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookingService bookingService, ILogger<BooksController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            _logger.LogInformation("Fetching list of all books.");
            var books = await _bookingService.GetAllBooksAsync();
            _logger.LogInformation($"Retrieved {books.Count} books.");
            var response = new ApiResponse<List<BookDto>>(StatusCodes.Status200OK, "Books retrieved successfully.", books);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            _logger.LogInformation($"Fetching book with ID: {id}");
            var book = await _bookingService.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status404NotFound, "Book not found.", $"No book found with ID {id}.");
                return NotFound(errorResponse);
            }

            _logger.LogInformation($"Book with ID {id} retrieved successfully.");
            var response = new ApiResponse<BookDto>(StatusCodes.Status200OK, "Book retrieved successfully.", book);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for adding a book.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Adding a new book.");
            var createdBook = await _bookingService.AddBookAsync(book);
            _logger.LogInformation($"Book with ID {createdBook.Id} created successfully.");
            var response = new ApiResponse<BookDto>(StatusCodes.Status201Created, "Book created successfully.", createdBook);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto updatedBook)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for updating a book.");
                return BadRequest(ModelState);
            }

            if (id != updatedBook.Id)
            {
                _logger.LogWarning($"Book ID mismatch: URL ID {id} does not match body ID {updatedBook.Id}.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid input.", "Book ID in the URL does not match the ID in the body.");
                return BadRequest(errorResponse);
            }

            _logger.LogInformation($"Updating book with ID: {id}");
            var updated = await _bookingService.UpdateBookAsync(updatedBook);
            if (updated == null)
            {
                _logger.LogWarning($"Book with ID {id} not found for update.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status404NotFound, "Book not found.", $"No book found with ID {id}.");
                return NotFound(errorResponse);
            }

            _logger.LogInformation($"Book with ID {id} updated successfully.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            _logger.LogInformation($"Deleting book with ID: {id}");
            var deleted = await _bookingService.DeleteBookAsync(id);
            if (!deleted)
            {
                _logger.LogWarning($"Book with ID {id} not found for deletion.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status404NotFound, "Book not found.", $"No book found with ID {id}.");
                return NotFound(errorResponse);
            }

            _logger.LogInformation($"Book with ID {id} deleted successfully.");
            return NoContent();
        }

        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> ReserveBook(int id, [FromBody] string comment)
        {
            _logger.LogInformation($"Reserving book with ID: {id}");
            var success = await _bookingService.ReserveBookAsync(id, comment);
            if (!success)
            {
                _logger.LogWarning($"Book with ID {id} is already reserved.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status409Conflict, "Book is already reserved.", $"Book with ID {id} is already reserved.");
                return Conflict(errorResponse);
            }

            _logger.LogInformation($"Book with ID {id} reserved successfully.");
            return NoContent();
        }

        [HttpPost("{id}/unreserve")]
        public async Task<IActionResult> UnreserveBook(int id)
        {
            _logger.LogInformation($"Unreserving book with ID: {id}");
            var success = await _bookingService.UnreserveBookAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Book with ID {id} is not reserved.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status400BadRequest, "Book is not reserved.", $"Book with ID {id} is not reserved.");
                return BadRequest(errorResponse);
            }

            _logger.LogInformation($"Book with ID {id} unreserved successfully.");
            return NoContent();
        }

        [HttpGet("reserved")]
        public async Task<IActionResult> GetReservedBooks()
        {
            _logger.LogInformation("Fetching list of reserved books.");
            var reservedBooks = await _bookingService.GetReservedBooksAsync();
            var response = new ApiResponse<List<BookDto>>(StatusCodes.Status200OK, "Reserved books retrieved successfully.", reservedBooks);
            return Ok(response);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableBooks()
        {
            _logger.LogInformation("Fetching list of available books.");
            var availableBooks = await _bookingService.GetAvailableBooksAsync();
            var response = new ApiResponse<List<BookDto>>(StatusCodes.Status200OK, "Available books retrieved successfully.", availableBooks);
            return Ok(response);
        }

        [HttpGet("{bookId}/history")]
        public async Task<IActionResult> GetStatusHistory(int bookId)
        {
            _logger.LogInformation($"Fetching status history for book with ID: {bookId}");
            var history = await _bookingService.GetBookStatusHistoryAsync(bookId);
            if (history == null || !history.Any())
            {
                _logger.LogWarning($"No status history found for book with ID {bookId}.");
                var errorResponse = new ApiErrorResponse(StatusCodes.Status404NotFound, "No status history found.", $"No status history found for book with ID {bookId}.");
                return NotFound(errorResponse);
            }

            _logger.LogInformation($"Status history for book with ID {bookId} retrieved successfully.");
            var response = new ApiResponse<List<BooksStatusHistoryDto>>(StatusCodes.Status200OK, "Status history retrieved successfully.", history);
            return Ok(response);
        }
    }
}