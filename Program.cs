using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new (
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        2,
        "Final Fantasy XIV",
        "Roleplaying",
        59.99M,
        new DateOnly(2021, 9, 30)
    ),
    new (
        3,
        "FIFA 23",
        "Sports",
        69.99M,
        new DateOnly(2022, 9, 27)
    ),
];

// GET /games
app.MapGet("games", () => games);

// GET /games/{id}
app.MapGet("games/{id}", (int id) =>
{
    GameDto? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(GetGameEndpointName);

// POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
    // CreateAtRoute generate a 201 Created HTTP response
    // Route Name: "GetGame" is the name given to the GET /games/{id} endpoint using .WithName("GetGame").
    // Route Values: new { id = game.Id } constructs the route values needed to generate the URI of the newly created game.
    // Response Body: game is the newly created GameDto object that is returned in the response body.
});

// PUT /games
app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);
    games[index] = new GameDto(
        id,
        updateGame.Name,
        updateGame.Genre,
        updateGame.Price,
        updateGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/1
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();
