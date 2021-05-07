using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using AutoMapper;
using pennywise.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Products.Commands.CreateProduct
{
    public partial class CreateProductCommand : IRequest<Response<long>>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<long>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<long>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(command);
            await _productRepository.AddAsync(product);
            return new Response<long>(product.Id);
        }
    }
}
