using System;
using System.Collections.Generic;
using System.Text;

namespace pennywise.Application.DTOs.Account
{
    public class UserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public double WalletBalance { get; set; }

    }
}
