using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.API.Controllers.Base;
using MyBlog.Common.Dto.Statistics;
using MyBlog.Common.Models.Statistics;
using MyBlog.Service.Abstract.Statistics;

namespace MyBlog.API.Controllers.Statistics;

[Route("api/stats")]
public class ContentStatisticsController : AppBaseController
{
    private readonly IContentStatisticsService _service;

    public ContentStatisticsController(IContentStatisticsService service)
    {
        _service = service;
    }

    [HttpPost("post/{postId:int:min(0)}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostStats(
        [FromRoute] [Required] int postId,
        [FromQuery] [Required] TimeMeasure measure,
        [FromBody] [Required] GetStatsForPostDto dto,
        CancellationToken ct)
    {
        var model = await _service.GetForPost(dto with { PostId = postId }, measure, ct);
        return Ok(model);
    }
}
