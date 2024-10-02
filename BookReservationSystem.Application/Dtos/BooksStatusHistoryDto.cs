namespace BookReservationSystem.Application.Models
{
    public class BooksStatusHistoryDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public bool IsReserved { get; set; }
        public string Comment { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
