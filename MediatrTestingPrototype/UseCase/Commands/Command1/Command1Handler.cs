using MediatR;
using MediatrTestingPrototype.Services;
using MediatrTestingPrototype.UseCase.Queries.Query1;

namespace MediatrTestingPrototype.UseCase.Commands.Command1;

public class Command1Handler(IMediator mediator, ISomeService someService) : IRequestHandler<Command1, Command1Response>
{
    public async Task<Command1Response> Handle(Command1 request, CancellationToken cancellationToken)
    {
        var priceQuery = await mediator.Send(new Query1 { Id = request.Id }, cancellationToken);
        var newPrice = priceQuery.Price + request.AddPrice;

        _ = await mediator.Send(new Command2.Command2
        {
            Id = request.Id,
            NewPrice = newPrice,
        }, cancellationToken);

        await someService.GetANumber(-100, 100);

        return new Command1Response { Price = newPrice };
    }
}
