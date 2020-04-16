﻿using PaymentAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Interfaces.Services
{
  public interface IIdentityService
  {
    Task<AuthenticationResult> RegisterAsync(string email, string password);
    Task<AuthenticationResult> LoginAsync(string email, string password);
  }
}
