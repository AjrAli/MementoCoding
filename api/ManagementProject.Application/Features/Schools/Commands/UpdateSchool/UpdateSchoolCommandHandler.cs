using AutoMapper;
using DotNetCore.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using ManagementProject.Application.Contracts.Persistence;
using ManagementProject.Application.Exceptions;
using ManagementProject.Application.Features.Response;
using ManagementProject.Application.Features.Schools.Commands.DeleteSchool;
using ManagementProject.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementProject.Application.Features.Students.Commands.UpdateStudent;

namespace ManagementProject.Application.Features.Schools.Commands.UpdateSchool
{
    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, UpdateSchoolCommandResponse>
    {
        private readonly IBaseRepository<School> _schoolRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseFactory<UpdateSchoolCommandResponse> _responseFactory;

        public UpdateSchoolCommandHandler(IMapper mapper,
                                          IBaseRepository<School> schoolRepository,
                                          IUnitOfWork unitOfWork,
                                          IResponseFactory<UpdateSchoolCommandResponse> responseFactory)
        {
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
            _responseFactory = responseFactory;
        }

        public async Task<UpdateSchoolCommandResponse> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var updateSchoolCommandResponse = _responseFactory.CreateResponse();
            await UpdateSchoolResponseHandling(request, updateSchoolCommandResponse);
            updateSchoolCommandResponse.Message = $"School {request.School?.Name} successfully updated";
            return updateSchoolCommandResponse;
        }

        private async Task UpdateSchoolResponseHandling(UpdateSchoolCommand request, UpdateSchoolCommandResponse updateSchoolCommandResponse)
        {

            var schoolToUpdate = await _schoolRepository.GetAsync(request?.School?.Id);
            _mapper.Map(request?.School, schoolToUpdate);
            await _schoolRepository.UpdateAsync(schoolToUpdate);
            if (await _unitOfWork.SaveChangesAsync() <= 0)
                throw new BadRequestException($"Failed to update school : {request?.School?.Name}");

        }
    }
}
