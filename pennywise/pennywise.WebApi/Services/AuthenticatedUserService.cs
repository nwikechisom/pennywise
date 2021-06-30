using pennywise.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using pennywise.Domain.Entities;

namespace pennywise.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public string UserId { get; }

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.FindByIdAsync(UserId);
        } 
    }
}
