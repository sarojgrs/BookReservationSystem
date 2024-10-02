using AutoMapper;
using BookReservationSystem.Application.Models;
using BookReservationSystem.Domain;

namespace ISR.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDto, Book>()
                    .ReverseMap();

            CreateMap<BooksStatusHistoryDto, BooksStatusHistory>()
                   .ReverseMap();
        }
    }
}
