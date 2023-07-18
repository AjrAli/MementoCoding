using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Contracts;
using ManagementProject.Domain.Common;
using ManagementProject.Domain.Entities;
using ManagementProject.Persistence.Configurations;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Persistence.Context
{
    public class ManagementProjectDbContext : DbContext
    {
        private readonly ILoggedInUserService? _loggedInUserService;
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public ManagementProjectDbContext(DbContextOptions<ManagementProjectDbContext> options)
            : base(options)
        {
        }

        public ManagementProjectDbContext(DbContextOptions<ManagementProjectDbContext> options, ILoggedInUserService loggedInUserService)
    : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }


        public DbSet<Student>? Students { get; set; }
        public DbSet<School>? Schools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // use loggerFactory
            optionsBuilder.UseLoggerFactory(_loggerFactory);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SchoolConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _loggedInUserService?.UserId ?? "";
                        entry.Entity.LastModifiedBy = _loggedInUserService?.UserId ?? "";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _loggedInUserService?.UserId ?? "";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
