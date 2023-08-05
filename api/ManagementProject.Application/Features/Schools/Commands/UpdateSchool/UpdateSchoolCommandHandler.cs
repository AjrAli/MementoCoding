using AutoMapper;
using MediatR;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;

namespace ManagementProject.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, UpdateSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSchoolCommandHandler(IMapper mapper, IBaseRepository<School> schoolRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateSchoolCommandResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolToUpdate = await _schoolRepository.GetAsync(request?.School?.Id);
            if (schoolToUpdate == null)
            {
                throw new NotFoundException(nameof(School), request?.School?.Id);
            }

            _mapper.Map(request?.School, schoolToUpdate);
            await _schoolRepository.UpdateAsync(schoolToUpdate);
            await _unitOfWork.SaveChangesAsync();

            return new UpdateSchoolCommandResponse
            {
                Message = $"School {request?.School?.Name} successfully updated"
            };
        }
    }
}
