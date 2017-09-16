﻿using JoyOI.ManagementService.Model.Dtos;
using JoyOI.ManagementService.Services;
using JoyOI.ManagementService.WebApi.WebApiModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JoyOI.ManagementService.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class DockerNodeController : Controller
    {
        private IDockerNodeService _dockerNodeService;

        public DockerNodeController(IDockerNodeService dockerNodeService)
        {
            _dockerNodeService = dockerNodeService;
        }

        [HttpGet("All")]
        public Task<ApiResponse<IList<DockerNodeOutputDto>>> Get()
        {
            IList<DockerNodeOutputDto> dtos = _dockerNodeService.GetNodes().ToList();
            return Task.FromResult(ApiResponse.OK(dtos));
        }
    }
}
