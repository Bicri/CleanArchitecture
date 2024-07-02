using AutoMapper;
using CleanArchitecture.Application.Contracts.Infraestructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Command.CreateStreamer;

public class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommand, int>
{
    //private readonly IStreamerRepository _streamerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CreateStreamerCommandHandler> _logger;

    public CreateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, ILogger<CreateStreamerCommandHandler> logger)
    {
        //_streamerRepository = streamerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
    {
        var streamerEntity = _mapper.Map<Streamer>(request);

        //var newStreamer = await _streamerRepository.AddAsync(streamerEntity);
        _unitOfWork.StreamerRepository.AddEntity(streamerEntity);

        var newStreamer = await _unitOfWork.Complete();

        if(newStreamer <= 0)
        {
            _logger.LogError("No se insertó el registro del streamer");
            throw new Exception($"No se pudo insertar el registro del streamer");
        }

        _logger.LogInformation($"Streamer {streamerEntity.Id} creado exitosamente");

        await SendEmail(streamerEntity);

        return streamerEntity.Id;
    }

    private async Task SendEmail(Streamer streamer)
    {
        Email email = new()
        {
            To = new List<DireccionEmail>() { new DireccionEmail { Email = "isaac.bicri@gmail.com" } },
            html = "La compañia de streamer se creó correctamente",
            Subject = "Creación de streamer"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error enviando el email de {streamer.Id} - {ex.Message}");
        }
    }
}
