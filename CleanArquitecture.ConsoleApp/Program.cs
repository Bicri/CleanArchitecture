
using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using System.IO;

StreamerDbContext dbContext = new();

//await AddNewRecords();
//await QueryStreaming();
//await QueryFilter();
//await QueryMethods();
//await QueryLink();
//await Tracking();
//await AddNewStreamerWithVideo();
//await AddNewStreamerWithVideoId();
//await AddNewActorWithVideo();
//await AddDirectorWithVideo();
await MultipleEntitiesQuery();

async Task MultipleEntitiesQuery()
{
    //var videoWithActores = await dbContext.Videos!.Include(a => a.Actores).FirstOrDefaultAsync(q => q.Id == 1);

    //var actor = await dbContext.Actores!.Select(m => m.Nombre).ToListAsync();
    var videoWithDirector = await dbContext.Videos!
                                    .Where(m => m.Director != null)
                                    .Include(m => m.Director)
                                    .Select(m => new
                                    {
                                        Director_Nombre_Completo = $"{m.Director!.Nombre} {m.Director.Apellido}",
                                        Movie = m.Nombre
                                    })
                                    .ToListAsync();

    foreach(var movie in videoWithDirector)
    {
        Console.WriteLine($"{movie.Movie} by {movie.Director_Nombre_Completo}");
    }
}

async Task AddDirectorWithVideo()
{
    var director = new Director
    {
        Nombre = "Lorenzo",
        Apellido = "Basteri",
        VideoId = 1
    };

    await dbContext.AddAsync(director);
    await dbContext.SaveChangesAsync();
}

async Task AddNewActorWithVideo()
{
    var actor = new Actor
    {
        Nombre = "Brad",
        Apellido = "Pitt"
    };

    await dbContext.AddAsync(actor);
    await dbContext.SaveChangesAsync();

    var videoActor = new VideoActor
    {
        ActorId = actor.Id,
        VideoId = 1
    };

    await dbContext.AddAsync(videoActor);
    await dbContext.SaveChangesAsync();
}

async Task AddNewStreamerWithVideoId()
{
    var batmanForever = new Video
    {
        Nombre = "Batman Forever",
        StreamerId = 2
    };

    await dbContext.AddAsync(batmanForever);
    await dbContext.SaveChangesAsync();
}
async Task AddNewStreamerWithVideo()
{
    var pantaya = new Streamer
    {
        Nombre = "Pantaya"
    };

    var hungerGames = new Video
    {
        Nombre = "Hunger Games",
        Streamer = pantaya
    };

    await dbContext.AddAsync(hungerGames);
    await dbContext.SaveChangesAsync();
}

async Task Tracking()
{
    var streamingTracking = await dbContext.Streamers!.AsTracking().FirstOrDefaultAsync(x => x.Id == 1);

    var streamingNoTracking = await dbContext.Streamers!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);
}

async Task QueryLink()
{
    var streamers = await (from i in dbContext.Streamers
                    select i).ToListAsync();

    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task QueryMethods()
{
    var streamer = dbContext!.Streamers!;

    var streamer1 = await streamer.Where(y => y.Nombre!.Contains("a")).FirstAsync();

    var streamer2 = await streamer.Where(y => y.Nombre!.Contains("a")).FirstOrDefaultAsync();

    var streame3 = await streamer.FirstOrDefaultAsync(x => x.Nombre!.Contains("a"));

    var streamer4 = await streamer.SingleAsync(x => x.Id == 1); // SOLO HAY UN SOLO RESULTADO, NO EL PRIMERO DE UNA LISTA

    var streamer5 = await streamer.SingleOrDefaultAsync(x => x.Id == 1); // EL VALOR O NULO

    var resultado = await streamer.FindAsync(1); // Por primary key
}

async Task QueryFilter()
{
    Console.WriteLine("Ingrese una compañía de straming: ");
    var streamingName = Console.ReadLine();
    //var streamer = await dbContext!.Streamers!.Where(x => x.Nombre!.Contains(streamingName!)).FirstOrDefaultAsync();

    var streamer = await dbContext!.Streamers!.Where(x => EF.Functions.Like(x.Nombre, $"%{streamingName}%")).FirstOrDefaultAsync();

    if (streamer is null)
    {
        Console.WriteLine("Streamer no encontrado");
        return;
    }

    Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
}

async Task QueryStreaming()
{
    var streamers = await dbContext!.Streamers!.ToListAsync();

    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task AddNewRecords()
{
    Streamer streamer = new()
    {
        Nombre = "Disney Plus",
        Url = "https://disney.com"
    };

    dbContext!.Streamers!.Add(streamer);

    await dbContext.SaveChangesAsync();


    List<Video> movies = new()
    {
        new Video
        {
            Nombre = "La cenicienta",
            StreamerId = streamer.Id
        },
        new Video
        {
            Nombre = "Cien mil un dalmatas",
            StreamerId = streamer.Id
        },
        new Video
        {
            Nombre = "El jorobado de notredam",
            StreamerId = streamer.Id
        },
        new Video
        {
            Nombre = "star wars",
            StreamerId = streamer.Id
        }
    };

    await dbContext!.AddRangeAsync(movies);
    await dbContext.SaveChangesAsync();
}
