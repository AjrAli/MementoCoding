using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementProject.Domain.Entities;

namespace ManagementProject.Persistence.Configurations
{
    public class SchoolConfiguration : BaseEntityConfiguration<School>
    {
        public override void Configure(EntityTypeBuilder<School> builder)
        {
            base.Configure(builder);

            builder.ToTable("Schools");

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Adress).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Town).IsRequired().HasMaxLength(100);
        }
    }
}
