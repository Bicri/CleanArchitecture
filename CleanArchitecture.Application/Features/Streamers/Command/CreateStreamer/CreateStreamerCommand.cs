using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Command.CreateStreamer;

public class CreateStreamerCommand : IRequest<int>
{
    public string Nombre { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
