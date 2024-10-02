using BookReservationSystem.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore.Storage;

namespace ISR.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        IDbContextTransaction BeginTransaction();
        Task<int> SaveChangesAsync();
    }
}
