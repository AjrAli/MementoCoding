using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementProject.Application.Features.Schools.Commands.CreateSchool
{
    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, CreateSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSchoolCommandHandler(IMapper mapper, IBaseRepository<School> schoolRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSchoolCommandResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = _mapper.Map<School>(request.School);
            await _schoolRepository.AddAsync(school);
            await _unitOfWork.SaveChangesAsync();

            return new CreateSchoolCommandResponse
            {
                Message = $"School {request.School?.Name} successfully created"
            };
        }
    }
}
