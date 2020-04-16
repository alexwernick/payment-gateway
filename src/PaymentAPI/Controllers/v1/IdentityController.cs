using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Contracts;
using PaymentAPI.Contracts.v1.Requests.Identity;
using PaymentAPI.Contracts.v1.Responses.Identity;
using PaymentAPI.Interfaces.Services;

namespace PaymentAPI.Controllers
{
  public class IdentityController : Controller
  {

    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
      _identityService = identityService;
    }

    [HttpPost(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {

      if (!ModelState.IsValid)
      {
        return BadRequest(new RegistrationResponse()
        {
          Success = false,
          Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
        });

      }

      var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

      if (!authResponse.Success)
      {
        return BadRequest(new RegistrationResponse()
        {
          Success = false,
          Errors = authResponse.Errors
        });
      }

      return Ok(new RegistrationResponse()
      {
        Success = true,
        Token = authResponse.Token
      });
    }

    [HttpPost(ApiRoutes.Identity.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

      if (!authResponse.Success)
      {
        return BadRequest(new LoginResponse()
        {
          Success = false,
          Errors = authResponse.Errors
        });
      }

      return Ok(new LoginResponse()
      {
        Success = true,
        Token = authResponse.Token
      });
    }

  }
}