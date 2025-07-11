using Acme.BookStore.Enums;
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
    public class BookFluentConfig : IEntityTypeConfiguration<Acme.BookStore.Models.Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(b => b.Type)
                .IsRequired()
                .HasConversion<string>(); // Convert enum to string for storage
            builder.Property(b => b.PublishDate)
                .IsRequired();
            builder.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // Use decimal for currency values
            builder.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // If an author is deleted, their books will also be deleted
            builder.ToTable("Books"); // Specify the table name
            builder.HasIndex(b => b.Name) // Create an index on Name for faster lookups
                .HasDatabaseName("IX_Book_Name")
                .IsUnique(false); // This index is not unique, as multiple books can have the same name
            builder.HasKey(b => b.Id); // Ensure the primary key is set
            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd(); // Ensure the Id is generated on add
            builder.Property(b => b.PublishDate)
                .HasColumnType("datetime"); // Specify the column type for PublishDate
            builder.Property(b => b.Price)
                .HasColumnType("decimal(18,2)"); // Specify the column type for Price
            builder.Property(b => b.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (BookType)Enum.Parse(typeof(BookType), v))
                .IsRequired(); // Ensure the enum is stored as a string and is required
            builder.HasIndex(b => new { b.Name, b.AuthorId }) // Create a composite index on Name and AuthorId for faster lookups
                .HasDatabaseName("IX_Book_Name_AuthorId")
                .IsUnique(false); // This index is not unique, as multiple books can have the same name and author

        }
    }
}
