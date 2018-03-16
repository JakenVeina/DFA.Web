using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DFA.Web.Models.Entities
{
    public class DFAWebDataContext : DbContext
    {
        /**********************************************************************/
        #region Constructors

        public DFAWebDataContext(DbContextOptions<DFAWebDataContext> options)
            : base(options) { }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        public DbSet<LogLevel> LogLevels { get; set; }

        public DbSet<LogAction> LogActions { get; set; }

        public DbSet<LogEntry> LogEntries { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<LoginCredential> LoginCredentials { get; set; }

        public DbSet<ExternalLoginPolicy> ExternalLoginPolicies { get; set; }

        public DbSet<ExternalLoginCredential> ExternalLoginCredentials { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRoleMap> UserRoleMaps { get; set; }

        public DbSet<NewsPost> NewsPosts { get; set; }

        public DbSet<UnreadNewsPostNotice> UnreadNewsPostNotices { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region Methods

        public bool IsAttached<TEntity>(TEntity entity) where TEntity : class
            => Set<TEntity>().Local.Contains(entity);

        public async Task UpdateEntityProperty<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class
        {
            var isAttached = IsAttached(entity);
            if (!isAttached)
                Attach(entity);

            var entityEntry = Entry(entity);
            entityEntry.Property(propertyExpression).IsModified = true;
            await SaveChangesAsync();

            if (!isAttached)
                entityEntry.State = EntityState.Detached;
        }

        #endregion Methods

        /**********************************************************************/
        #region DbContext

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ExternalLoginCredential>()
                .HasKey(x => new { x.UserId, x.PolicyId });

            modelBuilder
                .Entity<UserRoleMap>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder
                .Entity<UnreadNewsPostNotice>()
                .HasKey(x => new { x.NewsPostId, x.UserId });
        }

        #endregion DbContext
    }
}
