using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Contracts.MediatR.Command;
using ManagementProject.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManagementProject.Application.Features.PipelineBehaviours;

public class TransactionBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ManagementProjectDbContext _context;
    private readonly ILogger<TransactionBehavior<TCommand, TResponse>> _logger;

    public TransactionBehavior(ILogger<TransactionBehavior<TCommand, TResponse>> logger, ManagementProjectDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _logger.LogInformation("Starting a database transaction for {RequestName}", typeof(TCommand).Name);
            var response = await next();
            _logger.LogInformation("Committing the database transaction for {RequestName}", typeof(TCommand).Name);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction for {RequestName}", typeof(TCommand).Name);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }
}