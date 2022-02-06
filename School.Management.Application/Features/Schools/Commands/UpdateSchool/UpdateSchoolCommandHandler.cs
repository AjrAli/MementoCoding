using AutoMapper;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.EntityFrameworkCore;
namespace SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool
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
            var updateSchoolCommandResponse = new UpdateSchoolCommandResponse();
            var validator = new UpdateSchoolCommandValidator();
            try
            {
                var schoolToUpdate = await _schoolRepository.GetAsync(request.School.Id);
                if (schoolToUpdate == null)
                    throw new NotFoundException(nameof(School), request.School.Id);


                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.Errors.Count > 0)
                {
                    updateSchoolCommandResponse.Success = false;
                    updateSchoolCommandResponse.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors)
                    {
                        updateSchoolCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                    }
                }
                if (updateSchoolCommandResponse.Success)
                {
                    _mapper.Map(request.School, schoolToUpdate);

                    await _schoolRepository.UpdateAsync(schoolToUpdate);
                    if (await _unitOfWork.SaveChangesAsync() > 0)
                        updateSchoolCommandResponse.Message = "ok";
                    else
                        updateSchoolCommandResponse.Message = "Not ok";

                }
            }
            catch (Exception ex)
            {
                updateSchoolCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
            return updateSchoolCommandResponse;
        }
    }
}
