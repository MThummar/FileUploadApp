using Microsoft.EntityFrameworkCore;
namespace FileUploadApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the FileRecords table.
    /// </summary>
    public DbSet<FileRecord> FileRecords { get; set; }

    /// <summary>
    /// Configures the model for the context.
    /// </summary>
    /// <param name="modelBuilder">The builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Call the base method if using EF Core conventions

        // Configure FileRecord entity, this code will automatically generates unique Id for file records
        modelBuilder.Entity<FileRecord>(entity =>
        {
            entity.HasKey(e => e.Id); // Set Id as the primary key
            entity.Property(e => e.Id)
                          .ValueGeneratedOnAdd(); // Auto-generate Id values

        });

        // You can also configure other entities here if needed
    }
}



