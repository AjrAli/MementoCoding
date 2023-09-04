﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //Request
            _logger?.LogInformation($"Handling {typeof(TRequest)?.Name}");
            Type? myTypeRequest = request?.GetType();
            if (myTypeRequest != null)
            {
                IList<PropertyInfo>? propsOfMyRequest = myTypeRequest?.GetProperties()?.ToList();
                if (propsOfMyRequest != null)
                {
                    foreach (PropertyInfo prop in propsOfMyRequest)
                    {
                        if (request != null)
                        {
                            object? propValue = prop?.GetValue(request, null);
                            _logger?.LogInformation("{Property} : {@Value}", prop?.Name, propValue);
                        }
                    }
                    //Response
                    _logger?.LogInformation($"Handled {typeof(TResponse).Name}");
                }
            }
            return await next();
        }
    }
}
