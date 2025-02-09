using MediatR;

namespace MediatrTestingPrototype.Behaviors;

public static class ExceptionBehavior
{
    public static bool ThrowException { get; set; } = false;


    public class Exception : System.Exception
    {
        public Exception() { }
        public Exception(string message) : base(message) { }
        public Exception(string message, Exception inner) : base(message, inner) { }
    }
}

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (ExceptionBehavior.ThrowException)
        {
            throw new ExceptionBehavior.Exception();
        }

        return await next();
    }
}