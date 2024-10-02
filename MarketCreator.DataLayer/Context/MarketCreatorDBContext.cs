

using MarketCreator.DataLayer.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace MarketCreator.DataLayer.Context
{
    public class MarketCreatorDBContext:DbContext
    {
        #region Constructor
        public MarketCreatorDBContext(DbContextOptions<MarketCreatorDBContext> options):base(options) { }

        #endregion


        //یه لحظه ویدیو رو یعنی ضبط ویدیو رو نگه داشتم برم ببینم منبعم رو یعنی چک کنم درست نوشتم یا نه   :|
        //البته خیلی اینکارو میکنم در طول آموزش
        #region On Model Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s=>s.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Cascade;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region DbSets
        public DbSet<User> Users { get; set; }
        #endregion
    }
}
