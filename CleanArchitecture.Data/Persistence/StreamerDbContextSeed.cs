using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infraestructure.Persistence;

public class StreamerDbContextSeed
{
    public static async Task SeedAsync(StreamerDbContext context, ILogger<StreamerDbContext> logger)
    {
        if (!context.Streamers!.Any())
        {
            context.Streamers!.AddRange(GetPreConfiguredStreamer());
            await context.SaveChangesAsync();
            logger.LogInformation("Insertando semillas al db {context}", typeof(StreamerDbContext).Name);
        }
    }

    public static IEnumerable<Streamer> GetPreConfiguredStreamer()
    {
        return new List<Streamer>()
        {
            new Streamer
            {
                CreatedBy = "Semilla",
                CreatedDate = DateTime.Now,
                Nombre = "Maxi HBP",
                Url = "http://maxihbp.com"
            },
            new Streamer
            {
                CreatedBy = "Semilla",
                CreatedDate = DateTime.Now,
                Nombre = "Amazon VIP",
                Url = "http://amazonvip.com"
            }
        };
    }
}
