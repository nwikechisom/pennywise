using pennywise.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using System.Threading.Tasks;
using System.Threading;

namespace pennywise.Application.Features.Transact.Commands
{
    public class InitiateTransactionCommandValidator : AbstractValidator<InitiateTransactionCommand>
    {
        private readonly ITransactionRepositoryAsync transactionRepository;

        public InitiateTransactionCommandValidator(ITransactionRepositoryAsync transactionRepository)
        {
            this.transactionRepository = transactionRepository;

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");


            RuleFor(p => p.Amount)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

        }
    }
}
