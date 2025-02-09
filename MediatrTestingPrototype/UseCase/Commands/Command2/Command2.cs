using MediatR;

namespace MediatrTestingPrototype.UseCase.Commands.Command2;

public class Command2 : IRequest<Command2Response>
{
    public required int Id { get; init; }
    public required decimal NewPrice { get; init; }
}
