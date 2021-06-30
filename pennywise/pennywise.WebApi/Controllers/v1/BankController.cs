using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pennywise.Application.Features.BankDetails.Commands;
using pennywise.Application.Features.Banks.Commands;
using pennywise.Application.Features.Banks.Queries;

namespace pennywise.WebApi.Controllers.v1
{
    
    [ApiVersion("1.0")]
    [Authorize]
    public class BankController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllBanksParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllBanksQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        [HttpPost("resolveaccount")]
        public async Task<IActionResult> ResolveAccount([FromQuery] ResolveAccountNumberCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        
        [HttpPost("addbankdetail")]
        public async Task<IActionResult> AddBankDetail([FromQuery] CreateBankDetailCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}