
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PO_Task.Domain.Users;

namespace Aswaq.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(u =>
            u.Profile, profileBuilder =>
        {
            profileBuilder.ToTable("user_profile");
            profileBuilder.WithOwner().HasForeignKey("user_id");
            profileBuilder.Property(p => p.FirstName)
                .HasMaxLength(200)
                .HasConversion(firstName => firstName.Value, value => new FirstName(value));

            profileBuilder.Property(p => p.LastName)
                .HasMaxLength(200)
                .HasConversion(firstName => firstName.Value, value => new LastName(value));

            profileBuilder.Property(p => p.Email)
                .HasMaxLength(400)
                .HasConversion(email => email.Value, value => new Email(value));

            profileBuilder.HasIndex(p => p.Email).IsUnique();

            builder.HasIndex(user => user.Id).IsUnique();
        });
    }
}
