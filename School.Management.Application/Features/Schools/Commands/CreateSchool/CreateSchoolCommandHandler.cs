using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandlingService _responseHandlingService;
        public CreateSchoolCommandHandler(IMapper mapper, IBaseRepository<School> schoolRepository, IUnitOfWork unitOfWork, IResponseHandlingService responseHandlingService)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
            _responseHandlingService = responseHandlingService;
        }

        public async Task<CreateSchoolCommandResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var createSchoolCommandResponse = new CreateSchoolCommandResponse();
            var validator = new CreateSchoolCommandValidator();
            await CreateSchoolResponseHandling(request, createSchoolCommandResponse, validator);
            return createSchoolCommandResponse;
        }

        private async Task CreateSchoolResponseHandling(CreateSchoolCommand request, CreateSchoolCommandResponse createSchoolCommandResponse, CreateSchoolCommandValidator validator)
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                _responseHandlingService.ValidateRequestResult(createSchoolCommandResponse, validationResult);
                if (createSchoolCommandResponse.Success)
                {
                    var school = _mapper.Map<School>(request.School);

                    await _schoolRepository.AddAsync(school);
                    if (await _unitOfWork.SaveChangesAsync() <= 0)
                        createSchoolCommandResponse.Success = false;
                }
            }
            catch (Exception ex)
            {
                createSchoolCommandResponse.Success = false;
                createSchoolCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
        }
    }
}
