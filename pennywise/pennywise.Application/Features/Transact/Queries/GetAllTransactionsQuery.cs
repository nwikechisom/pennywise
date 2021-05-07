using AutoMapper;
using MediatR;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using pennywise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Transact.Queries
{
    public class GetAllTransactionsQuery : IRequest<PagedResponse<IEnumerable<GetAllTransactionsViewModel>>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, PagedResponse<IEnumerable<GetAllTransactionsViewModel>>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllTransactionsQueryHandler(ITransactionRepositoryAsync transactionRepositoryAsync, IMapper mapper)
        {
            _transactionRepositoryAsync = transactionRepositoryAsync;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IEnumerable<GetAllTransactionsViewModel>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllTransactionsParameter>(request);
            var transaction = await _transactionRepositoryAsync.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            if (!string.IsNullOrEmpty(request.UserId))
                transaction = (IReadOnlyList<Transaction>)transaction.Where(_ => _.UserId == request.UserId);
            var transactionViewModel = _mapper.Map<IEnumerable<GetAllTransactionsViewModel>>(transaction);
            return new PagedResponse<IEnumerable<GetAllTransactionsViewModel>>(transactionViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
