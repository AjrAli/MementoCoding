using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ManagementProject.Application.Features.PipelineBehaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling {RequestType}", typeof(TRequest).Name);
                var myTypeRequest = request?.GetType();
                IList<PropertyInfo>? propsOfMyRequest = myTypeRequest?.GetProperties()?.ToList();
                if (propsOfMyRequest != null)
                {
                    foreach (var prop in propsOfMyRequest)
                        _logger.LogInformation(
                            $"Property Name : {prop?.Name}, property type : {prop?.PropertyType}");
                }

                // Continue with the next behavior/handler in the pipeline
                var response = await next();

                _logger.LogInformation("Handled {RequestType}", typeof(TRequest).Name);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {RequestType}", typeof(TRequest).Name);
                throw;
            }
        }
    }
}