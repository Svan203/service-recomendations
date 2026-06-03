namespace ContentRecommendationService.Models;

public class RecommendationItem : ContentItem
{
    public int Score { get; set; }
    public List<string> Reasons { get; set; } = [];
}

