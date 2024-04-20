using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Command.DeleteStreamer;

public class DeleteStreamerCommand : IRequest
{
    public int Id { get; set; }
}
