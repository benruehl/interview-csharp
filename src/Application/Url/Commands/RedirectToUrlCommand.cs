using FluentValidation;
using HashidsNet;
using MediatR;
using UrlShortenerService.Application.Common.Exceptions;
using UrlShortenerService.Application.Common.Interfaces;

namespace UrlShortenerService.Application.Url.Commands;

public record RedirectToUrlCommand : IRequest<string>
{
    public string Id { get; init; } = default!;
}

public class RedirectToUrlCommandValidator : AbstractValidator<RedirectToUrlCommand>
{
    public RedirectToUrlCommandValidator()
    {
        _ = RuleFor(v => v.Id)
          .NotEmpty()
          .WithMessage("Id is required.");

        _ = RuleFor(v => v.Id)
          .Must(v => int.TryParse(v, out var val) && val > 0)
          .WithMessage("Id must be a positive integer.");
    }
}

public class RedirectToUrlCommandHandler : IRequestHandler<RedirectToUrlCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public RedirectToUrlCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> Handle(RedirectToUrlCommand request, CancellationToken cancellationToken)
    {
        var id = long.Parse(request.Id);
        var urlEntity = await _context.Urls.FindAsync(id, cancellationToken);

        if (urlEntity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Url), id);
        }

        return urlEntity.OriginalUrl;
    }
}
