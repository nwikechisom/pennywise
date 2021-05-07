﻿using pennywise.Application.Exceptions;
using pennywise.Application.Interfaces.Repositories;
using pennywise.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pennywise.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Response<long>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<long>>
        {
            private readonly IProductRepositoryAsync _productRepository;
            public UpdateProductCommandHandler(IProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<long>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(command.Id);

                if (product == null)
                {
                    throw new ApiException($"Product Not Found.");
                }
                else
                {
                    product.Name = command.Name;
                    product.Rate = command.Rate;
                    product.Description = command.Description;
                    await _productRepository.UpdateAsync(product);
                    return new Response<long>(product.Id);
                }
            }
        }
    }
}
