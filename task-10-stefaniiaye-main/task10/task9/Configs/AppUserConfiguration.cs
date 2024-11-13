using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using task9.Entities;

namespace task9.Configs;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(x => x.IdUser);
        builder
            .Property(x => x.IdUser)
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder
            .Property(x => x.Login)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(x => x.Password)
            .IsRequired();

        builder.ToTable("AppUser");
    }
}