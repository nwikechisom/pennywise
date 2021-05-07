using Microsoft.Extensions.Options;
using pennywise.Application.Interfaces;
using pennywise.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Infrastructure.Shared.Services
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly PaystackSettings _paystackSettings;

        public ExternalAuthService(IOptions<PaystackSettings> paystackSettings)
        {
            _paystackSettings = paystackSettings.Value;
        }
        public string GetPaystackToken()
        {
            return _paystackSettings.SecretKey;
        }
    }
}
