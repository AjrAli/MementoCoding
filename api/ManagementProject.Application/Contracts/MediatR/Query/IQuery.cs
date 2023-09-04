using MediatR;

namespace ManagementProject.Application.Contracts.MediatR.Query;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}