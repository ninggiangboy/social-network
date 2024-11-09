namespace Application.DTOs.Profiles;

public class StatsResponse
{
    public int TotalPosts { get; set; }
    public int TotalLikes { get; set; }
    public int DaysActive { get; set; }
    public int Recent30DaysPostsCount { get; set; }
    public int Recent30DaysLikesCount { get; set; }
    public List<DailyStats> DailyStats { get; set; } = [];
}

public class DailyStats
{
    public DateOnly Date { get; set; }
    public int Posts { get; set; }
    public int Likes { get; set; }
}