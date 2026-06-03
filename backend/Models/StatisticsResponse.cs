namespace ContentRecommendationService.Models;

public class StatisticsResponse
{
    public int Viewed { get; set; }
    public int Likes { get; set; }
    public int Ratings { get; set; }
    public List<InterestItem> PopularCategories { get; set; } = [];
    public List<InterestItem> PopularTags { get; set; } = [];
    public List<int> InteractedContentIds { get; set; } = [];
}

public class InterestItem
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}

