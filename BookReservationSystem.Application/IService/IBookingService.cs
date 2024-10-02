using BookReservationSystem.Application.Models;

namespace BookReservationSystem.Service.IService
{
    public interface IBookingService
    {
        Task<List<BookDto>> GetAllBooksAsync();

        Task<BookDto> GetBookByIdAsync(int id);

        Task<BookDto> AddBookAsync(BookDto book);

        Task<BookDto> UpdateBookAsync(BookDto updatedBook);

        Task<bool> DeleteBookAsync(int id);

        Task<bool> ReserveBookAsync(int bookId, string comment);

        Task<bool> UnreserveBookAsync(int bookId);

        Task<List<BookDto>> GetReservedBooksAsync();

        Task<List<BookDto>> GetAvailableBooksAsync();

        Task<List<BooksStatusHistoryDto>> GetBookStatusHistoryAsync(int bookId);
    }

}
