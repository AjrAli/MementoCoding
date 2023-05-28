using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolProject.Management.Application.Contracts.Persistence;
using SchoolProject.Management.Application.Exceptions;
using SchoolProject.Management.Application.Features.Response;
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
        private readonly IResponseFactory<UpdateSchoolCommandResponse> _responseFactory;

        public UpdateSchoolCommandHandler(IMapper mapper,
                                          ILogger<UpdateSchoolCommand> logger,
                                          IBaseRepository<School> schoolRepository,
                                          IUnitOfWork unitOfWork,
                                          IResponseFactory<UpdateSchoolCommandResponse> responseFactory)
        {
            _mapper = mapper;
            _logger = logger;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
            _responseFactory = responseFactory;
        }

        public async Task<UpdateSchoolCommandResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var updateSchoolCommandResponse = _responseFactory.CreateResponse();
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
                var exception = new BadRequestException("Update school failed!", ex);
                updateSchoolCommandResponse.Success = false;
                updateSchoolCommandResponse.Message = exception.ExceptionStr;
                throw exception;
            }
        }
    }
}
