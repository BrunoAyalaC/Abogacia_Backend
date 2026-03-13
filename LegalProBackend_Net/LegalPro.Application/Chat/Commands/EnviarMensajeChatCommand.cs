using FluentValidation;
using MediatR;
using LegalPro.Application.Common.Interfaces;

namespace LegalPro.Application.Chat.Commands;

public record EnviarMensajeChatCommand(string History, string UserInput) : IRequest<ChatResult>;
public record ChatResult(string Respuesta);

public class EnviarMensajeChatValidator : AbstractValidator<EnviarMensajeChatCommand>
{
    public EnviarMensajeChatValidator()
    {
        RuleFor(x => x.UserInput).NotEmpty().WithMessage("El mensaje no puede estar vacío.");
    }
}

public class EnviarMensajeChatHandler : IRequestHandler<EnviarMensajeChatCommand, ChatResult>
{
    private readonly IGeminiService _geminiService;

    public EnviarMensajeChatHandler(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    public async Task<ChatResult> Handle(EnviarMensajeChatCommand request, CancellationToken cancellationToken)
    {
        var resultJson = await _geminiService.ChatLegalAsync(request.History, request.UserInput);
        var doc = System.Text.Json.JsonDocument.Parse(resultJson);
        var root = doc.RootElement;
        var respuesta = root.TryGetProperty("respuesta", out var r) ? r.GetString() ?? "" : resultJson;

        return new ChatResult(respuesta);
    }
}
