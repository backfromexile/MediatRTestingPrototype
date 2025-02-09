using MediatR;

namespace MediatrTestingPrototype.UseCase.Queries.Query1;

public class Query1 : IRequest<Query1Response>
{
    public required int Id { get; init; }
}
