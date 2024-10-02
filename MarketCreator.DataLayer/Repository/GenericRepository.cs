
using MarketCreator.DataLayer.Context;
using MarketCreator.DataLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace MarketCreator.DataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        #region Constructor
        private readonly MarketCreatorDBContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(MarketCreatorDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        #endregion


        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(long entityId)
        {
            return await _dbSet.AsQueryable().SingleOrDefaultAsync(s => s.Id == entityId);
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public void EditEntity(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            entity.LastUpdateDate= DateTime.Now;
            EditEntity(entity);
        }

        public async Task DeleteEntityAsync(long entityId)
        {
            TEntity entity = await GetByIdAsync(entityId);
            if (entity != null)
                DeleteEntity(entity);
        }

        public void DeletePermanent(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeletePermanentAsync(long entityId)
        {
            TEntity entity=await GetByIdAsync(entityId);
            if(entity != null)
                DeletePermanent(entity);
        }



        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        #endregion
    }
}
