using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.Interfaces
{
    public interface IExternalAuthService
    {
        string GetPaystackToken();
    }
}
