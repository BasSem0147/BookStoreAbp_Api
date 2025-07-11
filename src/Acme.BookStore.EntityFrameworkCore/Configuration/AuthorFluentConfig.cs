using Acme.BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Configuration
{
    public class AuthorFluentConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.Surname)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.Bio)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(a => a.Picture)
                .HasMaxLength(500);
            builder.Property(a => a.BirthDate)
                .IsRequired();
            builder.Property(a => a.DeathDate)
                .IsRequired(false); // DeathDate can be null if the author is still alive
            builder.HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // If an author is deleted, their books will also be deleted
            builder.ToTable("Authors"); // Specify the table name
            builder.HasIndex(a => new { a.Name, a.Surname }) // Create an index on Name and Surname for faster lookups
                .HasDatabaseName("IX_Author_Name_Surname")
                .IsUnique(false); // This index is not unique, as multiple authors can have the same name and surname
            builder.HasKey(a => a.Id); // Ensure the primary key is set
            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd(); // Ensure the Id is generated on add
        }
    }
}
