using ContentRecommendationService.Models;

namespace ContentRecommendationService.Services;

public class RecommendationRepository
{
    private readonly List<ContentItem> _contentItems =
    [
        new() { Id = 1, Title = "Interstellar", Description = "A science fiction movie about space, time, and human survival.", Category = "Movies", Tags = ["science", "movie", "technology"], Rating = 4.8, Views = 320 },
        new() { Id = 2, Title = "Black Mirror", Description = "A series about technology, society, and unexpected digital consequences.", Category = "Series", Tags = ["technology", "science", "cyber security"], Rating = 4.6, Views = 290 },
        new() { Id = 3, Title = "How Artificial Intelligence Works", Description = "A beginner-friendly article about AI concepts and machine learning.", Category = "Articles", Tags = ["AI", "technology", "education"], Rating = 4.7, Views = 410 },
        new() { Id = 4, Title = "Web Development Basics", Description = "A practical video introduction to HTML, CSS, and JavaScript.", Category = "Videos", Tags = ["programming", "web", "frontend"], Rating = 4.3, Views = 250 },
        new() { Id = 5, Title = "React for Beginners", Description = "A course that teaches components, props, state, and modern frontend structure.", Category = "Courses", Tags = ["programming", "frontend", "education"], Rating = 4.5, Views = 360 },
        new() { Id = 6, Title = "Cybersecurity on the Internet", Description = "An article about safe browsing, passwords, scams, and data protection.", Category = "Articles", Tags = ["cyber security", "technology", "education"], Rating = 4.4, Views = 205 },
        new() { Id = 7, Title = "Inception", Description = "A movie about dreams, memory, and layered realities.", Category = "Movies", Tags = ["movie", "science", "technology"], Rating = 4.7, Views = 335 },
        new() { Id = 8, Title = "C# and ASP.NET Core Basics", Description = "A video that explains backend development with C# and Web API.", Category = "Videos", Tags = ["programming", "backend", "web"], Rating = 4.6, Views = 280 },
        new() { Id = 9, Title = "Databases for Beginners", Description = "A course about tables, relationships, SQL, and storing application data.", Category = "Courses", Tags = ["database", "backend", "education"], Rating = 4.2, Views = 190 },
        new() { Id = 10, Title = "Recommendation Systems", Description = "An article about content-based filtering and personalized suggestions.", Category = "Articles", Tags = ["AI", "database", "technology"], Rating = 4.9, Views = 430 },
        new() { Id = 11, Title = "Frontend Design Principles", Description = "A guide to building clean layouts, readable interfaces, and useful UI states.", Category = "Articles", Tags = ["frontend", "web", "education"], Rating = 4.1, Views = 165 },
        new() { Id = 12, Title = "Cloud Backend Overview", Description = "A short course about APIs, servers, hosting, and backend architecture.", Category = "Courses", Tags = ["backend", "technology", "web"], Rating = 4.3, Views = 225 }
    ];

    private readonly List<UserAction> _actions = [];
    private int _nextActionId = 1;

    public IReadOnlyList<ContentItem> GetAllContent()
    {
        return _contentItems;
    }

    public UserAction? SaveAction(int contentId, ActionType actionType, int? ratingValue = null)
    {
        var item = _contentItems.FirstOrDefault(content => content.Id == contentId);
        if (item is null)
        {
            return null;
        }

        if (actionType == ActionType.View)
        {
            item.Views++;
        }

        if (actionType == ActionType.Rating && ratingValue.HasValue)
        {
            item.Rating = Math.Round((item.Rating + ratingValue.Value) / 2, 1);
        }

        var action = new UserAction
        {
            Id = _nextActionId++,
            ContentItemId = contentId,
            ActionType = actionType,
            RatingValue = ratingValue,
            CreatedAt = DateTime.UtcNow
        };

        _actions.Add(action);
        return action;
    }

    public List<RecommendationItem> GetRecommendations()
    {
        if (_actions.Count == 0)
        {
            return [];
        }

        var interactedIds = _actions.Select(action => action.ContentItemId).ToHashSet();
        var sourceProfiles = BuildSourceProfiles();

        // Recommendation scoring: stronger actions create stronger user interests,
        // then unseen content receives points for matching categories and shared tags.
        return _contentItems
            .Where(content => !interactedIds.Contains(content.Id))
            .Select(content => ScoreContent(content, sourceProfiles))
            .Where(recommendation => recommendation.Score > 0)
            .OrderByDescending(recommendation => recommendation.Score)
            .ThenByDescending(recommendation => recommendation.Rating)
            .ThenByDescending(recommendation => recommendation.Views)
            .Take(6)
            .ToList();
    }

    public StatisticsResponse GetStatistics()
    {
        var categoryCounts = new Dictionary<string, int>();
        var tagCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var action in _actions)
        {
            var item = _contentItems.FirstOrDefault(content => content.Id == action.ContentItemId);
            if (item is null)
            {
                continue;
            }

            var weight = GetActionWeight(action);
            categoryCounts[item.Category] = categoryCounts.GetValueOrDefault(item.Category) + weight;

            foreach (var tag in item.Tags)
            {
                tagCounts[tag] = tagCounts.GetValueOrDefault(tag) + weight;
            }
        }

        return new StatisticsResponse
        {
            Viewed = _actions.Count(action => action.ActionType == ActionType.View),
            Likes = _actions.Count(action => action.ActionType == ActionType.Like),
            Ratings = _actions.Count(action => action.ActionType == ActionType.Rating),
            PopularCategories = ToInterestList(categoryCounts),
            PopularTags = ToInterestList(tagCounts),
            InteractedContentIds = _actions.Select(action => action.ContentItemId).Distinct().ToList()
        };
    }

    private List<UserInterestProfile> BuildSourceProfiles()
    {
        return _actions
            .Select(action =>
            {
                var content = _contentItems.First(item => item.Id == action.ContentItemId);
                return new UserInterestProfile(content, GetActionWeight(action));
            })
            .Where(profile => profile.Weight > 0)
            .ToList();
    }

    private RecommendationItem ScoreContent(ContentItem candidate, List<UserInterestProfile> sourceProfiles)
    {
        var score = 0;
        var reasons = new HashSet<string>();

        foreach (var profile in sourceProfiles)
        {
            if (candidate.Category == profile.Content.Category)
            {
                score += profile.Weight + 2;
                reasons.Add($"same category: {candidate.Category}");
            }

            var sharedTags = candidate.Tags
                .Intersect(profile.Content.Tags, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (sharedTags.Count > 0)
            {
                score += profile.Weight + sharedTags.Count;
                foreach (var tag in sharedTags)
                {
                    reasons.Add($"shared tag: {tag}");
                }
            }
        }

        return new RecommendationItem
        {
            Id = candidate.Id,
            Title = candidate.Title,
            Description = candidate.Description,
            Category = candidate.Category,
            Tags = candidate.Tags,
            Rating = candidate.Rating,
            Views = candidate.Views,
            Score = score,
            Reasons = reasons.Take(3).ToList()
        };
    }

    private static int GetActionWeight(UserAction action)
    {
        return action.ActionType switch
        {
            ActionType.View => 1,
            ActionType.Like => 3,
            ActionType.Rating when action.RatingValue == 4 => 4,
            ActionType.Rating when action.RatingValue == 5 => 5,
            ActionType.Rating => 1,
            _ => 0
        };
    }

    private static List<InterestItem> ToInterestList(Dictionary<string, int> source)
    {
        return source
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Key)
            .Take(5)
            .Select(item => new InterestItem { Name = item.Key, Count = item.Value })
            .ToList();
    }

    private sealed record UserInterestProfile(ContentItem Content, int Weight);
}

