using MyBlog.Common.Dto.Statistics;
using MyBlog.Common.Models.Statistics;

namespace MyBlog.Service.Abstract.Statistics;

public interface IContentStatisticsService
{
    Task<PostActivityModel> GetForPost(
        GetStatsForPostDto dto,
        TimeMeasure measure,
        CancellationToken ct = default);
}