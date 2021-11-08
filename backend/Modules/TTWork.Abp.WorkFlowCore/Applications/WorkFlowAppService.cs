using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TTWork.Abp.WorkFlowCore.Applications.Dtos;
using TTWork.Abp.WorkFlowCore.Models;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services.DefinitionStorage;

namespace TTWork.Abp.WorkFlowCore.Applications
{
    [AbpAuthorize(AppPermissions.Pages.Administration.Default)]
    public class WorkflowAppService : AsyncCrudAppService<PersistedWorkflow, WorkflowDto, Guid, AppResultRequestDto, WorkflowDto, WorkflowDto>
    {
        private readonly IRepository<PersistedExecutionError, Guid> _executionErrorRepository;
        private readonly IWorkflowController _workflowController;
        private readonly IDefinitionLoader _definitionLoader;

        public WorkflowAppService(
            IRepository<PersistedWorkflow, Guid> repository,
            IRepository<PersistedExecutionError, Guid> executionErrorRepository,
            IWorkflowController workflowController,
            IDefinitionLoader definitionLoader) : base(repository)
        {
            _executionErrorRepository = executionErrorRepository;
            _workflowController = workflowController;
            _definitionLoader = definitionLoader;
        }


        [Obsolete]
        public override Task<WorkflowDto> CreateAsync(WorkflowDto input)
        {
            throw new Exception("NotUsed");
            return base.CreateAsync(input);
        }

        [Obsolete]
        public override Task<WorkflowDto> UpdateAsync(WorkflowDto input)
        {
            throw new Exception("NotUsed");
            return base.UpdateAsync(input);
        }

        public override Task DeleteAsync(EntityDto<Guid> input)
        {
            return base.DeleteAsync(input);
        }
        
        

        public async Task<object> Excute(string name)
        {
            var ans = await _workflowController.StartWorkflow(name);
            return ans;
        }


        public async Task<string> ExcuteDSL(string json)
        {
            var loader = _definitionLoader.LoadDefinition(json, Deserializers.Json);

            var ans = await _workflowController.StartWorkflow(loader.Id);
            return ans;
        }

        public override async Task<PagedResultDto<WorkflowDto>> GetAllAsync(AppResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);
            foreach (var dto in result.Items)
            {
                if (dto.Status == 0)
                {
                    var id = dto.Id.ToString();
                    var errors = await _executionErrorRepository.GetAll().AsNoTracking().Where(x => x.WorkflowId == id).ToListAsync();
                    foreach (var error in errors)
                    {
                        dto.ExecutionErrors.Add(new ExecutionErrorDto
                        {
                            ErrorTime = error.ErrorTime,
                            Message = error.Message
                        });
                    }
                }
            }

            return result;
        }


        protected override IQueryable<PersistedWorkflow> ApplySorting(IQueryable<PersistedWorkflow> query, AppResultRequestDto input)
        {
            return base.ApplySorting(query, input);
        }

        protected override IQueryable<PersistedWorkflow> CreateFilteredQuery(AppResultRequestDto input)
        {
            return base.CreateFilteredQuery(input)
                    // .Include(x => x.ExecutionPointers)
                    .WhereIf(input.IsActive is true, x => x.Status == WorkflowStatus.Runnable || x.Status == WorkflowStatus.Suspended)
                    .WhereIf(input.IsActive is false, x => x.Status == WorkflowStatus.Complete || x.Status == WorkflowStatus.Terminated)
                    .WhereIf(!input.Keyword.IsNullOrEmptyOrWhiteSpace(), x => x.WorkflowDefinitionId.Contains(input.Keyword))
                ;
        }
    }
}