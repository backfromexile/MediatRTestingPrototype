using MediatrTestingPrototype.Behaviors;
using MediatrTestingPrototype.Services;
using MediatrTestingPrototype.UseCase.Commands.Command1;
using MediatrTestingPrototype.UseCase.Commands.Command2;
using MediatrTestingPrototype.UseCase.Queries.Query1;
using Moq;
using Shouldly;

namespace MediatrTestingPrototype.Tests.UseCase.Commands;

public class Command1Test : CommandTestBaseClass
{
    private readonly Mock<ISomeService> _someServiceMock = new();

    [Fact]
    public async Task Test1()
    {
        MediatorMock.Setup(m => m.Send(It.Is<Query1>(q => q.Id == 1), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Query1Response { Price = 1.5m })
            .Verifiable();

        MediatorMock.Setup(m => m.Send(It.Is<Command2>(c => c.Id == 1 && c.NewPrice == 1.6m), It.IsAny<CancellationToken>()))
            .Verifiable();

        _someServiceMock.Setup(s => s.GetANumber(-100, 100))
            .ReturnsAsync(5)
            .Verifiable();

        var response = await Mediator.Send(new Command1 { Id = 1, AddPrice = 0.1m });

        response.Price.ShouldBe(1.6m);

        MediatorMock.Verify();
        _someServiceMock.Verify();
    }

    [Fact]
    public async Task Test2()
    {
        ExceptionBehavior.ThrowException = true;

        await Mediator.Send(new Command1 { Id = 0, AddPrice = 0m })
            .ShouldThrowAsync<ExceptionBehavior.Exception>();
    }
}