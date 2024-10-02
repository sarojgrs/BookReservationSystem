using System.ComponentModel.DataAnnotations;

namespace BookReservationSystem.Domain
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsReserved { get; set; }
        public string ReservationComment { get; set; }
    }
}
