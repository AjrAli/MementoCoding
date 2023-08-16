using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Features.Response;
using System;
using Microsoft.Extensions.Logging;

namespace ManagementProject.Application.Features.PipelineBehaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<IRequest<TResponse>> _logger;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<IRequest<TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults?.SelectMany(r => r.Errors)?.Where(f => f != null)?.ToList();
                if (failures != null && failures?.Count > 0)
                {
                    var realResponseType = typeof(TResponse);
                    if (Activator.CreateInstance(realResponseType) is IBaseResponse errorResponse)
                    {
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
            return await next();
        }
    }
}
