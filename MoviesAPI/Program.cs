using MoviesAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MoviesDatabaseSettings>(builder.Configuration.GetSection("MoviesDatabaseSettings"));
builder.Services.AddSingleton<MoviesService>();

var app = builder.Build();

app.MapGet("/", () => "Movies API!");

app.MapGet("/api/movies", async (MoviesService moviesService) => await moviesService.Get());
app.MapGet("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
    var movie = await moviesService.GetById(id);
    if(movie == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(movie);
});


app.MapPost("/api/movies", async (MoviesService moviesService, Movie movie) =>
{
    await moviesService.Create(movie);
    return Results.Ok();
});

app.MapPut("/api/movies/{id}", async (MoviesService moviesService, string id,Movie updatedMovie) =>
{
    var movie = await moviesService.GetById(id);
    if(movie is null)
    {
        return Results.NotFound();
    }
    updatedMovie.Id = movie.Id;
    await moviesService.Update(id, updatedMovie);

    return Results.NoContent();
});

app.MapDelete("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
    var movie = await moviesService.GetById(id);
    if (movie is null)
    {
        return Results.NotFound();
    }
    await moviesService.Remove(id);

    return Results.NoContent();
});

app.Run();