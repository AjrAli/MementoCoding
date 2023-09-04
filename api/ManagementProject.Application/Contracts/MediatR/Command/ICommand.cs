using MediatR;

namespace ManagementProject.Application.Contracts.MediatR.Command;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}