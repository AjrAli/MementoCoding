using AutoMapper;
using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Students.Commands.CreateStudent
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentCommandHandler(IMapper mapper, IBaseRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateStudentCommandResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = _mapper.Map<Student>(request.Student);
            await _studentRepository.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return new CreateStudentCommandResponse
            {
                Message = $"Student {request?.Student?.FirstName} successfully created"
            };
        }
    }
}
