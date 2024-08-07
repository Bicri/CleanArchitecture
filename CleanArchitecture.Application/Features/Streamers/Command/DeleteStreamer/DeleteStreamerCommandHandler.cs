﻿using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Command.DeleteStreamer;

public class DeleteStreamerCommandHandler : IRequestHandler<DeleteStreamerCommand>
{
    //private readonly IStreamerRepository _streamerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteStreamerCommandHandler> _logger;

    public DeleteStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteStreamerCommandHandler> logger)
    {
        //_streamerRepository = streamerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
    {
        //Streamer streamerToDelete = await _streamerRepository.GetByIdAsync(request.Id);
        Streamer streamerToDelete = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);

        if (streamerToDelete is null)
        {
            _logger.LogError($"{request.Id} no existe en el sistema");
            throw new NotFoundException(nameof(Streamer), request.Id);
        }

        //await _streamerRepository.DeleteAsync(streamerToDelete);
        _unitOfWork.StreamerRepository.DeleteEntity(streamerToDelete);
        await _unitOfWork.Complete();

        _logger.LogInformation($"El {request.Id} streamer fue eliminado con éxito");

        return Unit.Value;
    }
}
