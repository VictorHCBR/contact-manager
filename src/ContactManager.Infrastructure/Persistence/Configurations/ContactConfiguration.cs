using ContactManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManager.Infrastructure.Persistence.Configurations;

public sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(contact => contact.Id);

        builder.Property(contact => contact.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(contact => contact.BirthDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(contact => contact.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(contact => contact.IsActive)
            .IsRequired();

        builder.Property(contact => contact.CreatedAtUtc)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(contact => contact.UpdatedAtUtc)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasIndex(contact => new { contact.IsActive, contact.Name });
    }
}
