using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts;
using SchoolProject.Management.Domain.Common;
using SchoolProject.Management.Domain.Entities;
using SchoolProject.Management.Persistence.Configurations;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Context
{
    public class SchoolManagementDbContext : DbContext
    {
        private readonly ILoggedInUserService _loggedInUserService;
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public SchoolManagementDbContext(DbContextOptions<SchoolManagementDbContext> options)
            : base(options)
        {
        }

        public SchoolManagementDbContext(DbContextOptions<SchoolManagementDbContext> options, ILoggedInUserService loggedInUserService)
    : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<School> Schools { get; set; }

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
                        entry.Entity.CreatedBy = _loggedInUserService.UserId;
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _loggedInUserService.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
