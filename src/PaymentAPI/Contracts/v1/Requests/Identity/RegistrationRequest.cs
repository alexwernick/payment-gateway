﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts.v1.Requests.Identity
{
  public class RegistrationRequest
  {
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }

  }
}
