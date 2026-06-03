namespace ContentRecommendationService.Models;

public class UserAction
{
    public int Id { get; set; }
    public int ContentItemId { get; set; }
    public ActionType ActionType { get; set; }
    public int? RatingValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

