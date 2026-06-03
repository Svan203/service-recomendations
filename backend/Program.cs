using ContentRecommendationService.Models;
using ContentRecommendationService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<RecommendationRepository>();

var app = builder.Build();

app.UseCors("Frontend");

app.MapGet("/", () => Results.Ok(new { message = "Content Recommendation Service API" }));

app.MapGet("/api/content", (RecommendationRepository repository) =>
{
    return Results.Ok(repository.GetAllContent());
});

app.MapGet("/api/recommendations", (RecommendationRepository repository) =>
{
    return Results.Ok(repository.GetRecommendations());
});

app.MapPost("/api/actions/view/{contentId:int}", (int contentId, RecommendationRepository repository) =>
{
    var result = repository.SaveAction(contentId, ActionType.View);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.MapPost("/api/actions/like/{contentId:int}", (int contentId, RecommendationRepository repository) =>
{
    var result = repository.SaveAction(contentId, ActionType.Like);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.MapPost("/api/actions/rate/{contentId:int}", (int contentId, RateRequest request, RecommendationRepository repository) =>
{
    if (request.Rating < 1 || request.Rating > 5)
    {
        return Results.BadRequest(new { message = "Rating must be from 1 to 5." });
    }

    var result = repository.SaveAction(contentId, ActionType.Rating, request.Rating);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.MapGet("/api/statistics", (RecommendationRepository repository) =>
{
    return Results.Ok(repository.GetStatistics());
});

app.Run();
