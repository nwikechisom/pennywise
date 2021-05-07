using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pennywise.Application.Features.Transact.Commands;
using pennywise.Application.Features.Transact.Queries;
using pennywise.Application.Interfaces;

namespace pennywise.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class TransactionController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public TransactionController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> StartTransaction(InitiateTransactionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("CompletedAction")]
        [AllowAnonymous]
        public async Task<IActionResult> TransactionCallback([FromQuery]string trxref)
        {
            return Ok(await Mediator.Send(new CompleteTransactionCommand {  transactionReference = trxref }));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllTransactionsParameter filter)
        {
            return Ok(await Mediator.Send(new GetAllTransactionsQuery() { UserId = _authenticatedUserService.UserId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }
    }
}