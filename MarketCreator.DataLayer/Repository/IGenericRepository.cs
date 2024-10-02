

using MarketCreator.DataLayer.Entities.Common;
using System.Runtime.CompilerServices;

namespace MarketCreator.DataLayer.Repository
{
    public interface IGenericRepository<TEntity>: IAsyncDisposable where TEntity : BaseEntity
    {
        //Get, Add, Delete, Edit 

        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(long entityId);

        Task AddEntityAsync(TEntity entity);

        void EditEntity(TEntity entity);

        Task DeleteEntityAsync(long entityId);
        void DeleteEntity(TEntity entity);

        Task DeletePermanentAsync(long entityId);
        void DeletePermanent(TEntity entity);

        Task SaveChangesAsync();

    }
}
