using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
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
        private readonly ILogger<UpdateSchoolCommand> _logger;

        public UpdateSchoolCommandHandler(IMapper mapper,
                                          ILogger<UpdateSchoolCommand> logger,
                                          IBaseRepository<School> schoolRepository,
                                          IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateSchoolCommandResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var updateSchoolCommandResponse = new UpdateSchoolCommandResponse();
            await UpdateSchoolResponseHandling(request, updateSchoolCommandResponse);
            return updateSchoolCommandResponse;
        }

        private async Task UpdateSchoolResponseHandling(UpdateSchoolCommand request, UpdateSchoolCommandResponse updateSchoolCommandResponse)
        {
            try
            {
                var schoolToUpdate = await _schoolRepository.GetAsync(request?.School?.Id);
                if (schoolToUpdate == null)
                    throw new NotFoundException(nameof(School), request?.School?.Id ?? 0);

                _mapper.Map(request?.School, schoolToUpdate);
                await _schoolRepository.UpdateAsync(schoolToUpdate);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    updateSchoolCommandResponse.Success = false;
            }
            catch (Exception ex)
            {
                updateSchoolCommandResponse.Success = false;
                _logger.LogWarning($"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}");
                updateSchoolCommandResponse.Message = $"ERROR : {ex.Message} {ex.InnerException?.Source} : {ex.InnerException?.Message}";
                throw new BadRequestException("Update school failed!");
            }
        }
    }
}
