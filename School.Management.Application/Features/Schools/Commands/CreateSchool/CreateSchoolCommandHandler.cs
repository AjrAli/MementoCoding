using AutoMapper;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;
using System;

namespace SchoolProject.Management.Application.Features.Schools.Commands.CreateSchool
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
            var createSchoolCommandResponse = new CreateSchoolCommandResponse();

            var validator = new CreateSchoolCommandValidator();
            try
            {
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.Errors.Count > 0)
                {
                    createSchoolCommandResponse.Success = false;
                    createSchoolCommandResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors)
                    {
                        createSchoolCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                    }
                }
                if (createSchoolCommandResponse.Success)
                {
                    var school = _mapper.Map<School>(request.School);

                    await _schoolRepository.AddAsync(school);
                    if (await _unitOfWork.SaveChangesAsync() > 0)
                        createSchoolCommandResponse.Message = "ok";
                    else
                        createSchoolCommandResponse.Message = "Not ok";
                }
            }
            catch (Exception ex)
            {
                createSchoolCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
            return createSchoolCommandResponse;
        }
    }
}
