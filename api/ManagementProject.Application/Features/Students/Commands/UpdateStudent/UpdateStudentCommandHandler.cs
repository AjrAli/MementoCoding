using AutoMapper;
using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Students.Commands.UpdateStudent
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, UpdateStudentCommandResponse>
    {
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentCommandHandler(IMapper mapper, IBaseRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateStudentCommandResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var studentToUpdate = await _studentRepository.GetAsync(request?.Student?.Id);

            if (studentToUpdate == null)
            {
                throw new NotFoundException(nameof(Student), request?.Student?.Id);
            }

            _mapper.Map(request?.Student, studentToUpdate);
            await _studentRepository.UpdateAsync(studentToUpdate);
            await _unitOfWork.SaveChangesAsync();

            return new UpdateStudentCommandResponse
            {
                Message = $"Student {request?.Student?.FirstName} successfully updated"
            };
        }
    }
}
