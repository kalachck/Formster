using Microsoft.Extensions.DependencyInjection;

namespace Formster.Application.Sqrs;

public interface ICommand;

public interface IQuery<TResponse>;

public interface ICommandHandler
{
    Task HandleAsync(ICommand command, CancellationToken ct);
}

public interface ICommandHandler<in TCommand> : ICommandHandler
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TResponse>
{
    Task<TResponse> HandleAsync(IQuery<TResponse> query, CancellationToken ct);
}

public interface IQueryHandler<in TQuery, TResponse> : IQueryHandler<TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken ct);
}

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    public abstract Task HandleAsync(TCommand command, CancellationToken ct);

    async Task ICommandHandler.HandleAsync(ICommand command, CancellationToken ct)
        => await HandleAsync((TCommand)command, ct);
}

public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    public abstract Task<TResponse> HandleAsync(TQuery query, CancellationToken ct);

    public async Task<TResponse> HandleAsync(IQuery<TResponse> query, CancellationToken ct)
        => await HandleAsync((TQuery)query, ct);
}

public interface ICommandDispatcher
{
    Task DispatchAsync(ICommand command, CancellationToken ct);
}

public interface IQueryDispatcher
{
    Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct);
}

public interface ILocalMessageBus : ICommandDispatcher, IQueryDispatcher;

internal class LocalMessageBus : ILocalMessageBus
{
    private readonly IServiceProvider _serviceProvider;

    public LocalMessageBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task DispatchAsync(ICommand command, CancellationToken ct)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = (ICommandHandler)sp.GetRequiredService(handlerType);

            await handler.HandleAsync(command, ct);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }

    public async Task<TResponse> DispatchAsync<TResponse>(IQuery<TResponse> query, CancellationToken ct)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            var handler = (IQueryHandler<TResponse>)sp.GetRequiredService(handlerType);

            return await handler.HandleAsync(query, ct);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }
}
