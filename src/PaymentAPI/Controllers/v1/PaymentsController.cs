using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentAPI.Contracts;
using PaymentAPI.Contracts.v1.Requests.Payments;
using PaymentAPI.Contracts.v1.Responses.Payments;
using PaymentAPI.Contracts.v1.Responses.Payments.Enumerations;
using PaymentAPI.Domain;
using PaymentAPI.Extensions;
using PaymentAPI.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers.v1
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class PaymentsController : Controller
  {
    private readonly IPaymentService _paymentService;
    private readonly PaymentAPI.AcquiringBankClient.v1.Interfaces.Services.IBankPaymentService _bankPaymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService,
        PaymentAPI.AcquiringBankClient.v1.Interfaces.Services.IBankPaymentService bankPaymentService,
        ILogger<PaymentsController> logger)
    {
      _paymentService = paymentService;
      _bankPaymentService = bankPaymentService;
      _logger = logger;

    }

    [HttpGet(ApiRoutes.Payments.GetAll)]
    public async Task<IActionResult> GetAll()
    {
      var payments = await _paymentService.GetPaymentsByUserIdAsync(HttpContext.GetUserId());
      return Ok(new GetAllPaymentsResponse()
      {
        Payments = payments.Select(x => ConvertPaymentFromDomainToGetPaymentResponse(x)).ToList()
      });
    }

    [HttpGet(ApiRoutes.Payments.Get)]
    public async Task<IActionResult> Get([FromRoute]Guid paymentId)
    {
      var payment = await _paymentService.GetPaymentByIdAndUserIdAsync(paymentId, HttpContext.GetUserId());

      if (payment == null)
        return NotFound();

      return Ok(ConvertPaymentFromDomainToGetPaymentResponse(payment));
    }


    [HttpPost(ApiRoutes.Payments.Create)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest paymentRequest)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new CreatePaymentResponse()
        {
          Status = StatusType.Failed,
          Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)).ToList()
        });
      }

      var bankPayment = new PaymentAPI.AcquiringBankClient.v1.Contracts.Requests.BankPayments.CreateBankPaymentRequest()
      {
        CardNumber = paymentRequest.CardNumber,
        ExpiryDate = paymentRequest.ExpiryDate,
        Amount = paymentRequest.Amount,
        Cvv = paymentRequest.Cvv,
        NameOnCard = paymentRequest.NameOnCard
      };

      var bankPaymentResponse = await _bankPaymentService.CreatePaymentAsync(bankPayment);

      if (bankPaymentResponse.Id.HasValue)
        _logger.LogInformation($"Bank payment created with Id: {bankPaymentResponse.Id.Value}");

      var payment = new Payment()
      {
        CardNumber = paymentRequest.CardNumber,
        ExpiryDate = paymentRequest.ExpiryDate,
        Amount = paymentRequest.Amount,
        Cvv = paymentRequest.Cvv,
        NameOnCard = paymentRequest.NameOnCard,
        AddDate = DateTime.UtcNow,
        UserId = HttpContext.GetUserId(),
        AcquiringBankPaymentId = bankPaymentResponse.Id,
        Status = bankPaymentResponse.Success ? Domain.Enumerations.StatusType.Success : Domain.Enumerations.StatusType.Failed,
        AcquiringBankErrors = string.Join(",", bankPaymentResponse.Errors)
      };

      bool paymentCreateSuccess = false;

      try
      {
        paymentCreateSuccess = await _paymentService.CreatePaymentAsync(payment);
      }
      catch (Exception ex)
      {
        _logger.LogCritical(ex, "Payment failed to insert in to database. Possible Loss of customer data. External bank payment Id: {Id}", bankPaymentResponse.Id.HasValue ? bankPaymentResponse.Id.Value.ToString() : "");

        return StatusCode(StatusCodes.Status500InternalServerError, new CreatePaymentResponse()
        {
          Status = StatusType.Failed,
          Errors = new List<string>() { "Internal server error" }
        });
      }

      if (!paymentCreateSuccess)
      {
        _logger.LogCritical("Payment failed to insert in to database. Possible Loss of customer data. External bank payment Id: {Id}", bankPaymentResponse.Id.HasValue ? bankPaymentResponse.Id.Value.ToString() : "");

        return StatusCode(StatusCodes.Status500InternalServerError, new CreatePaymentResponse()
        {
          Status = StatusType.Failed,
          Errors = new List<string>() { "Internal server error" }
        });
      }

      _logger.LogInformation($"Payment inserted with Id: {payment.Id}");

      var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
      var locationUri = baseUrl + "/" + ApiRoutes.Payments.Get.Replace("{paymentId}", payment.Id.ToString());

      return Created(locationUri, new CreatePaymentResponse()
      {
        Status = MapStatusTypeFromDomainToContract(payment.Status),
        Id = payment.Id,
        Errors = bankPaymentResponse.Errors
      });
    }

    private StatusType MapStatusTypeFromDomainToContract(Domain.Enumerations.StatusType domainStatusType)
    {
      var contractStatusType = StatusType.Other;

      if (domainStatusType == Domain.Enumerations.StatusType.Success)
        contractStatusType = StatusType.Success;
      else if (domainStatusType == Domain.Enumerations.StatusType.Failed)
        contractStatusType = StatusType.Failed;

      return contractStatusType;
    }

    private GetPaymentResponse ConvertPaymentFromDomainToGetPaymentResponse(Payment payment)
    {
      return new GetPaymentResponse()
      {
        Id = payment.Id,
        CardNumber = payment.CardNumber,
        ExpiryDate = payment.ExpiryDate,
        Amount = payment.Amount,
        Cvv = payment.Cvv,
        NameOnCard = payment.NameOnCard,
        Status = MapStatusTypeFromDomainToContract(payment.Status),
        AddDate = payment.AddDate,
        AcquiringBankPaymentId = payment.AcquiringBankPaymentId
      };
    }
  }
}
