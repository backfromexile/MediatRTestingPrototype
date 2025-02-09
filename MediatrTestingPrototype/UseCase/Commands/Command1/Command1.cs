using MediatR;

namespace MediatrTestingPrototype.UseCase.Commands.Command1;

public class Command1 : IRequest<Command1Response>
{
    public required int Id { get; init; }
    public required decimal AddPrice { get; init; }
}
