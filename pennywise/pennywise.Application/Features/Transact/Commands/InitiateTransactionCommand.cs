using AutoMapper;
using MediatR;
using pennywise.Application.DTOs.Payment;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Transact.Commands
{
    public class InitiateTransactionCommand : IRequest<Response<InitiatePaymentResponse>>
    {
        public string Title { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
    }
    public class InitiateTransactionCommandHandler : IRequestHandler<InitiateTransactionCommand, Response<InitiatePaymentResponse>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepository;
        private readonly IMapper _mapper;
        public InitiateTransactionCommandHandler(ITransactionRepositoryAsync transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<Response<InitiatePaymentResponse>> Handle(InitiateTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = await _transactionRepository.StartNewTransaction(request);
            return response;
        }
    }
}
