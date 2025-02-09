namespace MediatrTestingPrototype.Services;

public interface ISomeService
{
    Task<int> GetANumber(int min = int.MinValue, int max = int.MaxValue);
}
