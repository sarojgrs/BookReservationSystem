using System.ComponentModel.DataAnnotations;

namespace BookReservationSystem.Application.Models
{
    public class BookDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }
        public bool IsReserved { get; set; } = false; 
        public string ReservationComment { get; set; } = string.Empty;
    }
}
