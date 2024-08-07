﻿using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Directors.Queries.CreateDirector;

public class CreateDirectorHandler : IRequestHandler<CreateDirectorCommand, int>
{
    private readonly ILogger<CreateDirectorHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDirectorHandler(ILogger<CreateDirectorHandler> logger, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
    {
        /*
            NOTA: Un video solo puede estar relacionado a un director, por lo que si se intenta agrgar un director con un video que ya tiene un director asignado,
            se lanzará una excepción.
         */
        var directorEntity = _mapper.Map<Director>(request);

        _unitOfWork.Repository<Director>().AddEntity(directorEntity);

        var result = await _unitOfWork.Complete();
        
        if(result <= 0)
        {
            _logger.LogError("No se insertó el registro del director");
            throw new Exception($"No se pudo insertar el registro del director");
        }

        return directorEntity.Id;
    }
}
