using pennywise.Application.Features.Products.Commands.CreateProduct;
using pennywise.Application.Features.Products.Queries.GetAllProducts;
using AutoMapper;
using pennywise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using pennywise.Application.Features.Transact.Commands;
using pennywise.Application.DTOs.Payment;
using pennywise.Application.Features.Plan.Commands.Create;
using pennywise.Application.Features.Plan.Queries;
using pennywise.Application.Features.Transact.Queries;
using pennywise.Application.Features.BankDetails.Queries;
using pennywise.Application.Features.BankDetails.Commands;
using pennywise.Application.Features.Banks.Queries;

namespace pennywise.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Product
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
            #endregion

            #region Transaction
            CreateMap<InitiateTransactionCommand, InitiatePaymentRequest>().ReverseMap();
            CreateMap<Transaction, InitiatePaymentRequest>().ReverseMap();
            CreateMap<InitiateTransactionCommand, Transaction>().ReverseMap();
            CreateMap<GetAllTransactionsParameter, GetAllTransactionsQuery>().ReverseMap();
            CreateMap<GetAllTransactionsViewModel, Transaction>().ReverseMap();
            #endregion

            #region Plan
            CreateMap<CreatePlanCommand, PaymentPlan>().ReverseMap();
            CreateMap<GetAllPlansViewModel, PaymentPlan>().ReverseMap();
            CreateMap<GetAllPlansQuery, GetAllPlansParameter>().ReverseMap();
            CreateMap<GetPlanDetailsViewModel, PaymentPlan>().ReverseMap();
            #endregion

            #region BankDetails
            CreateMap<BankDetail, GetAllBankDetailsViewModel>().ReverseMap();
            CreateMap<CreateBankDetailCommand, BankDetail>();
            CreateMap<GetAllBankDetailsQuery, GetAllBankDetailsParameter>();
            #endregion

            #region Banks
            CreateMap<Bank, GetAllBanksViewModel>().ReverseMap();
            CreateMap<GetAllBanksQuery, GetAllBanksParameter>();
            #endregion
        }
    }
}