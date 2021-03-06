﻿using JoyOI.ManagementService.Model.Dtos;
using JoyOI.ManagementService.Services;
using JoyOI.ManagementService.WebApi.WebApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyOI.ManagementService.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class StateMachineInstanceController : Controller
    {
        private IStateMachineInstanceService _stateMachineInstanceService;

        public StateMachineInstanceController(IStateMachineInstanceService stateMachineInstanceService)
        {
            _stateMachineInstanceService = stateMachineInstanceService;
        }

        [HttpGet("All")]
        public async Task<ApiResponse<IList<StateMachineInstanceOutputDto>>> All(
            [FromQuery]string name,
            [FromQuery]string stage,
            [FromQuery]string status,
            [FromQuery]string begin_time,
            [FromQuery]string finish_time)
        {
            var dtos = await _stateMachineInstanceService.Search(name, stage, status, begin_time, finish_time);
            return ApiResponse.OK(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<StateMachineInstanceOutputDto>> Get(Guid id)
        {
            var dto = await _stateMachineInstanceService.Get(id);
            if (dto == null)
                return ApiResponse.NotFound(Response, "state machine instance not found", dto);
            return ApiResponse.OK(dto);
        }

        [HttpPut]
        public async Task<ApiResponse<PutResult<Guid>>> Put([FromBody]StateMachineInstancePutDto dto)
        {
            var putResult = await _stateMachineInstanceService.Put(dto);
            var result = new PutResult<Guid>(putResult.Instance?.Id ?? Guid.Empty);
            return ApiResponse.Custom(Response, putResult.Code, putResult.Message, result);
        }

        [HttpPatch("{id}")]
        public async Task<ApiResponse<PatchResult>> Patch(
            Guid id, [FromBody]StateMachineInstancePatchDto dto)
        {
            var patchResult = await _stateMachineInstanceService.Patch(id, dto);
            var result = new PatchResult(patchResult.Code == 200 ? 1 : 0);
            return ApiResponse.Custom(Response, patchResult.Code, patchResult.Message, result);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<DeleteResult>> Delete(Guid id)
        {
            var deleted = await _stateMachineInstanceService.Delete(id);
            var result = new DeleteResult(deleted);
            if (deleted <= 0)
                return ApiResponse.NotFound(Response, "state machine instance not found", result);
            return ApiResponse.OK(result);
        }
    }
}
