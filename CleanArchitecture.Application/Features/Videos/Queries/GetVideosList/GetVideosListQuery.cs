using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;

public class GetVideosListQuery : IRequest<List<VideosVm>>
{
    public string _userName { get; set; } = string.Empty;

    public GetVideosListQuery(string username)
    {
        _userName = username ?? throw new ArgumentException(nameof(username));
    }
}
