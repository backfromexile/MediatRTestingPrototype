namespace MediatrTestingPrototype.Services;

internal class SomeService : ISomeService
{
    private readonly Random _random = new Random();

    public Task<int> GetANumber(int min = int.MinValue, int max = int.MaxValue) => Task.FromResult(_random.Next(min, max));
}