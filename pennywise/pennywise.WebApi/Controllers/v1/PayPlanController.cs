using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pennywise.Application.Features.Plan.Commands.Create;
using pennywise.Application.Features.Plan.Queries;
using pennywise.Application.Interfaces;

namespace pennywise.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PayPlanController : BaseApiController
    {
        private IAuthenticatedUserService _authenticatedUserService;

        public PayPlanController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }
        [HttpPost("CreatePlan")]
        public async Task<IActionResult> CreatePlan(CreatePlanCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllPlansParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllPlansQuery() { UserId = _authenticatedUserService.UserId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Ok(await Mediator.Send(new GetPlanDetailsQuery { PlanId = id }));
        }
    }
}