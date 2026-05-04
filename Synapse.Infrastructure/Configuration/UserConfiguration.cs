using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Synapse.Domain.Entities;

namespace Synapse.Infrastructure.Persistence;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username).IsRequired();
        builder.Property(x => x.Password).IsRequired();

        builder.HasIndex(x => x.Username).IsUnique();
    }
}