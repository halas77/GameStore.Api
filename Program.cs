using GameStore.Api.Entities;

const string GetGameEndpointName = "GetGame";

List<Game> games = new()
{
    new Game()
    {
       Id= 1,
       Name = "Vice city I",
       Genre = "Fighting",
       Price = 29.99M,
       ReleaseDate = new DateTime(1999, 2, 4),
       ImageUri = "https://placehold.co/100"
    },
    new Game()
    {
       Id= 2,
       Name = "Salanders IV",
       Genre = "Fighting",
       Price = 59.99M,
       ReleaseDate = new DateTime(1991, 7, 4),
       ImageUri = "https://placehold.co/100"
    },
    new Game()
    {
       Id= 3,
       Name = "Winning Eleven 8",
       Genre = "Sport",
       Price = 19.99M,
       ReleaseDate = new DateTime(2008, 9, 1),
       ImageUri = "https://placehold.co/100"
    },
};


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var group = app.MapGroup("/games")
               .WithParameterValidation();

// get games
group.MapGet("/", () => games);

// get game by id
group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(game);
})
.WithName(GetGameEndpointName);

// create game
group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// update game
group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? existedGame = games.Find(game => game.Id == id);

    if (existedGame is null)
    {
        return Results.NotFound();
    }

    existedGame.Name = updatedGame.Name;
    existedGame.Genre = updatedGame.Genre;
    existedGame.Price = updatedGame.Price;
    existedGame.ReleaseDate = updatedGame.ReleaseDate;
    existedGame.ImageUri = updatedGame.ImageUri;

    return Results.NoContent();

});


// delete game
app.MapDelete("/games/{id}", (int id) =>
{
    Game? existedGame = games.Find(game => game.Id == id);

    if (existedGame is not null)
    {
        games.Remove(existedGame);
    }
    return Results.NoContent();

});


app.Run();


