using AutoMapper;
using BookReservationSystem.Application.Models;
using BookReservationSystem.Infrastructure.Repository.Generic;
using BookReservationSystem.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookReservationSystem.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Domain.Book> _bookRepository;
        private readonly IRepository<Domain.BooksStatusHistory> _historyRepository;
        private readonly IMapper _mapper;

        // Constructor
        public BookingService(IRepository<Domain.Book> bookRepository, IRepository<Domain.BooksStatusHistory> historyRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            var bookEntities = (await _bookRepository.GetAllAsync()).ToList();
            var mappedEntities = _mapper.Map<List<BookDto>>(bookEntities);
            return mappedEntities;
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var bookEntity = await _bookRepository.GetByIdAsync(id);
            return _mapper.Map<BookDto>(bookEntity); // Map to the application model
        }

        public async Task<BookDto> AddBookAsync(BookDto book)
        {
            var bookEntity = _mapper.Map<Domain.Book>(book);
            await _bookRepository.AddAsync(bookEntity);
            await _bookRepository.SaveChangesAsync();
            return book; // Return the original book model
        }

        public async Task<BookDto> UpdateBookAsync(BookDto updatedBook)
        {
            var existingBook = await _bookRepository.GetByIdAsync(updatedBook.Id);
            if (existingBook == null)
            {
                return null; // Handle as needed
            }

            var bookEntity = _mapper.Map<Domain.Book>(updatedBook);
            _bookRepository.Update(bookEntity);
            await _bookRepository.SaveChangesAsync();
            return updatedBook; // Return the updated model
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return false; // Handle as needed
            }

            _bookRepository.Remove(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReserveBookAsync(int bookId, string comment)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.IsReserved)
            {
                return false; // Handle book not found or already reserved
            }

            var statusHistory = new Domain.BooksStatusHistory
            {
                BookId = bookId,
                IsReserved = true,
                Comment = comment,
                ChangedAt = DateTime.UtcNow
            };

            book.IsReserved = true;
            book.ReservationComment = comment;

            _bookRepository.Update(book);
            await _historyRepository.AddAsync(statusHistory);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnreserveBookAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || !book.IsReserved)
            {
                return false; // Handle book not found or not reserved
            }

            book.IsReserved = false;
            book.ReservationComment = string.Empty;

            var statusHistory = new Domain.BooksStatusHistory
            {
                BookId = bookId,
                IsReserved = false,
                Comment = "Unreserved",
                ChangedAt = DateTime.UtcNow
            };

            _bookRepository.Update(book);
            await _historyRepository.AddAsync(statusHistory);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<BookDto>> GetReservedBooksAsync()
        {
            var reservedBooks = await _bookRepository.FindAsync(b => b.IsReserved);
            return _mapper.Map<List<BookDto>>(reservedBooks.ToList());
        }

        public async Task<List<BookDto>> GetAvailableBooksAsync()
        {
            var availableBooks = await _bookRepository.FindAsync(b => !b.IsReserved);
            return _mapper.Map<List<BookDto>>(availableBooks.ToList());
        }

        public async Task<List<BooksStatusHistoryDto>> GetBookStatusHistoryAsync(int bookId)
        {
            var historyEntities = (await _historyRepository.FindAsync(h => h.BookId == bookId)).ToList();
            return _mapper.Map<List<BooksStatusHistoryDto>>(historyEntities);
        }
    }
}
