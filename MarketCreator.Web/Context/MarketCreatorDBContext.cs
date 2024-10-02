

using MarketCreator.DataLayer.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace MarketCreator.Web.Context
{
    public class MarketCreatorDBContext:DbContext
    {
        #region Constructor
        public MarketCreatorDBContext(DbContextOptions<MarketCreatorDBContext> options):base(options) { }

        #endregion



        #region DbSets
        public DbSet<User> Users { get; set; }
        #endregion
    }
}
