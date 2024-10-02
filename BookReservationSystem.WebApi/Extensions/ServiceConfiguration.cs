using BookReservationSystem.Infrastructure.Repository.Generic;
using BookReservationSystem.Service.IService;
using BookReservationSystem.Service.Services;
using ISR.Application.MappingProfiles;
using ISR.Infrastructure.UnitOfWork;

namespace BookReservationSystem.WebApi.Extensions
{
    public static class ServiceConfiguration
    {
        public static void ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookingService, BookingService>();
        }
    }
}
