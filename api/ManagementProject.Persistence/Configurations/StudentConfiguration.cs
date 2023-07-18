using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementProject.Domain.Entities;

namespace ManagementProject.Persistence.Configurations
{
    public class StudentConfiguration : BaseEntityConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            base.Configure(builder);

            builder.ToTable("Students");

            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Adress).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Age).IsRequired();

            // Relationships
            builder.HasOne(e => e.School).WithMany(e => e.Students)
                .HasForeignKey(e => e.SchoolId).IsRequired();
        }
    }
}
