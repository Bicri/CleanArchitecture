using AutoMapper;
using CleanArchitecture.Application.Features.Streamers.Command.CreateStreamer;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Video, VideosVm>();
        CreateMap<CreateStreamerCommand, Streamer>();
    }
}
