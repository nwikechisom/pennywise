using MediatR;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Transact.Commands
{
    public class CompleteTransactionCommand : IRequest<Response<string>>
    {
        public string transactionReference { get; set; }
    }
    public class CompleteTransactionCommandHandler : IRequestHandler<CompleteTransactionCommand, Response<string>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepository;
        public CompleteTransactionCommandHandler(ITransactionRepositoryAsync transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Response<string>> Handle(CompleteTransactionCommand command, CancellationToken cancellationToken)
        {
            var response = await _transactionRepository.VerifyTransaction(command.transactionReference);
            return response;
        }
    }
}
