using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Service;
using SchoolProject.Management.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolProject.Management.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, UpdateSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandlingService _responseHandlingService;

        public UpdateSchoolCommandHandler(IMapper mapper, IBaseRepository<School> schoolRepository, IUnitOfWork unitOfWork, IResponseHandlingService responseHandlingService)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
            _responseHandlingService = responseHandlingService;
        }

        public async Task<UpdateSchoolCommandResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var updateSchoolCommandResponse = new UpdateSchoolCommandResponse();
            var validator = new UpdateSchoolCommandValidator();
            await UpdateSchoolResponseHandling(request, updateSchoolCommandResponse, validator);
            return updateSchoolCommandResponse;
        }

        private async Task UpdateSchoolResponseHandling(UpdateSchoolCommand request, UpdateSchoolCommandResponse updateSchoolCommandResponse, UpdateSchoolCommandValidator validator)
        {
            try
            {
                var schoolToUpdate = await _schoolRepository.GetAsync(request?.School?.Id);
                if (schoolToUpdate == null)
                    throw new NotFoundException(nameof(School), request?.School?.Id ?? 0);

                var validationResult = await validator.ValidateAsync(request);
                _responseHandlingService.ValidateRequestResult(updateSchoolCommandResponse, validationResult);
                if (updateSchoolCommandResponse.Success)
                {
                    _mapper.Map(request?.School, schoolToUpdate);

                    await _schoolRepository.UpdateAsync(schoolToUpdate);
                    if (await _unitOfWork.SaveChangesAsync() <= 0)
                        updateSchoolCommandResponse.Success = false;

                }
            }
            catch (Exception ex)
            {
                updateSchoolCommandResponse.Success = false;
                updateSchoolCommandResponse.Message = $"ERROR : {ex.InnerException?.Source} : {ex.InnerException?.Message}";
            }
        }
    }
}
