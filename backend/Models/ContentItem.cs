namespace ContentRecommendationService.Models;

public class ContentItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public double Rating { get; set; }
    public int Views { get; set; }
}

