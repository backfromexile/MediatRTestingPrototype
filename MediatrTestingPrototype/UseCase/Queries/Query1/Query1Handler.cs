using MediatR;

namespace MediatrTestingPrototype.UseCase.Queries.Query1;

public class Query1Handler : IRequestHandler<Query1, Query1Response>
{
    public async Task<Query1Response> Handle(Query1 request, CancellationToken cancellationToken)
    {
        _ = await new ValueTask<bool>(true);

        return new Query1Response
        {
            Price = request.Id switch
            {
                1 => 1.5m,
                2 => 3m,
                3 => 0.50m,
                4 => 100m,
                _ => throw new InvalidOperationException(),
            },
        };
    }
}