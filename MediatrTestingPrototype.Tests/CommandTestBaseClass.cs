using MediatR;
using MediatrTestingPrototype.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Reflection;

namespace MediatrTestingPrototype.Tests;

public abstract class CommandTestBaseClass : IDisposable
{
    /// <summary>
    /// The <see cref="IMediator" /> instance that should be called in the tests.<br/>
    /// Using this instance will invoke the matching handler as well as registered <see cref="IPipelineBehavior{TRequest, TResponse}"/> behaviors.
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    /// A <see cref="IMediator" /> mock instance that is used for nested <see cref="IMediator"/> calls, for example if <see cref="IMediator"/> is used inside a request handler to send another request.
    /// </summary>
    protected Mock<IMediator> MediatorMock { get; } = new();

    private readonly IServiceCollection _serviceCollection;
    private readonly bool _canAddCustomMocks;
    private readonly IServiceScope _serviceScope;

    private static readonly MethodInfo _genericRegisterMockMethodInfo = typeof(CommandTestBaseClass).GetMethod(nameof(RegisterMock), BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException();


    protected CommandTestBaseClass()
    {
        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddApplicationMediator();

        _serviceCollection.RemoveAll<IMediator>();

        _serviceCollection.AddScoped(_ => MediatorMock.Object);

        _canAddCustomMocks = true;
        RegisterServiceMocks();
        _canAddCustomMocks = false;

        var serviceProvider = _serviceCollection.BuildServiceProvider();

        _serviceScope = serviceProvider.CreateScope();

        Mediator = new Mediator(_serviceScope.ServiceProvider);
    }

    /// <summary>
    /// Override this method to register service mocks manually.<br/>
    /// You need to call <see cref="RegisterServiceMock{T}"/> to register a service mock.<br/>
    /// This method will be called during the constructor.<br/>
    /// <br/>
    /// The default behaviour registers all mocks from fields of deriving classes where the field type is <see cref="Mock{T}"/>.
    /// </summary>
    protected virtual void RegisterServiceMocks() => RegisterMockFromFields();

    /// <summary>
    /// Creates a service mock and registers it for the tests.<br/>
    /// Calling this method anywhere outside of <see cref="RegisterServiceMocks"/> will throw an exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected Mock<T> RegisterServiceMock<T>() where T : class
    {
        if (!_canAddCustomMocks)
            throw new InvalidOperationException();

        var mock = new Mock<T>();

        RegisterMock(mock);

        return mock;
    }

    /// <summary>
    /// Register a mock instance manually. <br/>
    /// Calling this method anywhere outside of <see cref="RegisterServiceMocks"/> will throw an exception.
    /// </summary>
    protected void RegisterMock<T>(Mock<T> mock) where T : class
    {
        ArgumentNullException.ThrowIfNull(mock);

        if (!_canAddCustomMocks)
            throw new InvalidOperationException();

        _serviceCollection.AddScoped(_ => mock.Object);
    }

    private void RegisterMockFromFields()
    {
        var type = GetType();

        var mockFields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(field => field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Mock<>));

        foreach (var mockField in mockFields)
        {
            var mockedServiceType = mockField.FieldType.GetGenericArguments()[0];

            var mockInstance = mockField.GetValue(this);

            _genericRegisterMockMethodInfo.MakeGenericMethod(mockedServiceType).Invoke(this, [mockInstance]);
        }
    }

    void IDisposable.Dispose()
    {
        _serviceScope.Dispose();
        GC.SuppressFinalize(this);
    }
}
