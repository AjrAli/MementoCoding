using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Management.Domain.Common;

namespace SchoolProject.Management.Persistence.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Auto number primary key
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
            
            builder.Property(e => e.CreatedDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(e => e.CreatedDate).Metadata.GetBeforeSaveBehavior();
            builder.Property(e => e.CreatedDate).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(e => e.LastModifiedBy).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastModifiedBy).Metadata.GetBeforeSaveBehavior();
            builder.Property(e => e.LastModifiedBy).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(e => e.LastModifiedDate).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(e => e.LastModifiedDate).Metadata.GetBeforeSaveBehavior();
            builder.Property(e => e.LastModifiedDate).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


        }
    }
}
