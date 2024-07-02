using AutoMapper;
using CleanArchitecture.Application.Features.Directors.Queries.CreateDirector;
using CleanArchitecture.Application.Features.Streamers.Command.CreateStreamer;
using CleanArchitecture.Application.Features.Streamers.Command.UpdateStreamer;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Video, VideosVm>();
        CreateMap<CreateStreamerCommand, Streamer>();
        CreateMap<UpdateStreamerCommand, Streamer>();
        CreateMap<CreateDirectorCommand, Director>();
    }
}
