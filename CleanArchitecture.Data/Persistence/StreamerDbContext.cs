using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infraestructure.Persistence;

public class StreamerDbContext : DbContext
{
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Server=localhost; Database=Streamer;User ID=SA;Password=B1cr11999@;TrustServerCertificate=true")
    //        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
    //        .EnableSensitiveDataLogging();
    //}

    public StreamerDbContext(DbContextOptions<StreamerDbContext> options) : base(options)
    {  
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            switch(entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "system";
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "system";
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Streamer>()
            .HasMany(m => m.Videos)
            .WithOne(m => m.Streamer)
            .HasForeignKey(m => m.StreamerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Video>()
            .HasMany(m => m.Actores)
            .WithMany(m => m.Videos)
            .UsingEntity<VideoActor>(
                pt => pt.HasKey(e => new { e.ActorId, e.VideoId })
            );
    }

    public DbSet<Streamer>? Streamers { get; set; }
    public DbSet<Video>? Videos { get; set; }
    public DbSet<Actor>? Actores { get; set; }
    public DbSet<Director>? Directores { get; set; }
    public DbSet<VideoActor>? VideoActores { get; set; }
}
