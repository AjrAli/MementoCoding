using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Service;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool
{
    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, CreateSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSchoolCommand> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public CreateSchoolCommandHandler(IMapper mapper,
                                          ILogger<CreateSchoolCommand> logger,
                                          IBaseRepository<School> schoolRepository,
                                          IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSchoolCommandResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var createSchoolCommandResponse = new CreateSchoolCommandResponse();
            await CreateSchoolResponseHandling(request, createSchoolCommandResponse);
            return createSchoolCommandResponse;
        }

        private async Task CreateSchoolResponseHandling(CreateSchoolCommand request, CreateSchoolCommandResponse createSchoolCommandResponse)
        {
            try
            {
                var school = _mapper.Map<School>(request.School);
                await _schoolRepository.AddAsync(school);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    createSchoolCommandResponse.Success = false;
            }
            catch (Exception ex)
            {
                createSchoolCommandResponse.Success = false;
                _logger.LogWarning($"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}");
                createSchoolCommandResponse.Message = $"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}";
                throw new BadRequestException("Create school failed!");
            }
        }
    }
}
