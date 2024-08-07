﻿using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Command.UpdateStreamer;

public class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommand>
{
    //private readonly IStreamerRepository _streamerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateStreamerCommandHandler> _logger;

    public UpdateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateStreamerCommandHandler> logger)
    {
        //_streamerRepository = streamerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
    {
        //Streamer streamerToUpdate = await _streamerRepository.GetByIdAsync(request.Id);
        Streamer streamerToUpdate = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);

        if(streamerToUpdate is null)
        {
            _logger.LogError($"No se encontró el streamer Id: {request.Id}");
            throw new NotFoundException(nameof(Streamer), request.Id);
        }

        _mapper.Map(request, streamerToUpdate, typeof(UpdateStreamerCommand), typeof(Streamer));

        //await _streamerRepository.UpdateAsync(streamerToUpdate);
        _unitOfWork.StreamerRepository.UpdateEntity(streamerToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"La operación fue exitosa actulizando el streamer {request.Id}");

        return Unit.Value;
    }
}
