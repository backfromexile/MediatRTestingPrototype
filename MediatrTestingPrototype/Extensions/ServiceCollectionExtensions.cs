using MediatrTestingPrototype.Behaviors;
using MediatrTestingPrototype.Services;
using MediatrTestingPrototype.UseCase.Commands.Command1;
using Microsoft.Extensions.DependencyInjection;

namespace MediatrTestingPrototype.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddApplicationMediator();

        services.AddScoped<ISomeService, SomeService>();
    }

    public static void AddApplicationMediator(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Command1).Assembly);

            config.AddOpenBehavior(typeof(LoggingBehavior<,>))
                  .AddOpenBehavior(typeof(ExceptionBehavior<,>));
        });
    }
}
