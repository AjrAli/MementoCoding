using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Features.Response;
using System;
using ManagementProject.Application.Contracts.MediatR.Command;
using Microsoft.Extensions.Logging;

namespace ManagementProject.Application.Features.PipelineBehaviours
{
    public class ValidationBehaviour<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        private readonly IEnumerable<IValidator<TCommand>> _validators;
        private readonly ILogger<ValidationBehaviour<TCommand, TResponse>> _logger;
        public ValidationBehaviour(IEnumerable<IValidator<TCommand>> validators, ILogger<ValidationBehaviour<TCommand, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();
            var context = new ValidationContext<TCommand>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults?.SelectMany(r => r.Errors)?.Where(f => f != null)?.ToList();
            if (failures is not { Count: > 0 }) return await next();
            var realResponseType = typeof(TResponse);
            if (Activator.CreateInstance(realResponseType) is not IBaseResponse errorResponse)
                return await next();
            errorResponse.Success = false;
            errorResponse.ValidationErrors = new List<string>();
            foreach (var error in failures)
            {
                _logger?.LogError(error.ErrorMessage);
                errorResponse.ValidationErrors.Add(error.ErrorMessage);
            }
            return (TResponse)errorResponse;
        }
    }
}
