using ManagementProject.Application.Contracts.MediatR.Command;

namespace ManagementProject.Application.Models.Account.Command
{
    public class CreateSimpleUserCommand : ICommand<AccountResponse>
    {
        public AccountDto Account { get; set; } = new AccountDto();
    }
}
