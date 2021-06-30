using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using pennywise.Domain.Entities;

namespace pennywise.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        Task<ApplicationUser> GetCurrentUser();
    }
}
