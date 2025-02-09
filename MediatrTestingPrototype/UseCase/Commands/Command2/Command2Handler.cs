using MediatR;
using System.Diagnostics;

namespace MediatrTestingPrototype.UseCase.Commands.Command2;

public class Command2Handler : IRequestHandler<Command2, Command2Response>
{
    public async Task<Command2Response> Handle(Command2 request, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Changing price of {request.Id} to {request.NewPrice}.");

        await Task.Delay(0, cancellationToken);

        return new Command2Response { AffectedItems = 5 };
    }
}
