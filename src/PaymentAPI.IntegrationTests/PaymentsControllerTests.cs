using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentAPI.Contracts;
using PaymentAPI.Contracts.v1.Requests.Payments;
using PaymentAPI.Contracts.v1.Responses.Payments;
using PaymentAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.IntegrationTests
{
  [TestClass]
  public class PaymentsControllerTests : IntegrationTestBase
  {
    [TestMethod]
    public async Task GetAll_GetPaymentWithoutAnyPaymentsInDatabase_ReturnsEmptyResponse()
    {
      await AuthenticateAsync();

      var response = await _testClient.GetAsync(ApiRoutes.Payments.GetAll);

      Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
      Assert.IsTrue(!response.Content.ReadAsAsync<GetAllPaymentsResponse>().Result.Payments.Any());

    }

    [TestMethod]
    public async Task GetAll_GetPaymentsWithPaymentInDatabase_ReturnsPayment()
    {
      await AuthenticateAsync();

      var paymentRequest = new CreatePaymentRequest
      {
        CardNumber = "1111111111111111",
        ExpiryDate = DateTime.Now.AddYears(1),
        Amount = 100,
        Cvv = "111",
        NameOnCard = "test name"
      };

      await CreatePaymentAsync(paymentRequest); 

      var response = await _testClient.GetAsync(ApiRoutes.Payments.GetAll);
      var paymentsResponse = response.Content.ReadAsAsync<GetAllPaymentsResponse>().Result;
      
      Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
      Assert.AreEqual(paymentsResponse.Payments.Count(),1);
      Assert.AreEqual(paymentsResponse.Payments[0].CardNumber, GetMaskedCardNumber(paymentRequest.CardNumber));
      Assert.AreEqual(paymentsResponse.Payments[0].ExpiryDate, paymentRequest.ExpiryDate);
      Assert.AreEqual(paymentsResponse.Payments[0].Amount, paymentRequest.Amount);
      Assert.AreEqual(paymentsResponse.Payments[0].Cvv, paymentRequest.Cvv);
      Assert.AreEqual(paymentsResponse.Payments[0].NameOnCard, paymentRequest.NameOnCard);
    }

    [TestMethod]
    public async Task Get_GetPaymentWithPaymentInDatabase_ReturnsPayment()
    {
      await AuthenticateAsync();

      var paymentRequest = new CreatePaymentRequest
      {
        CardNumber = "1111111111111111",
        ExpiryDate = DateTime.Now.AddYears(1),
        Amount = 100,
        Cvv = "111",
        NameOnCard = "test name"
      };

      var createdPayment = await CreatePaymentAsync(paymentRequest);

      var response = await _testClient.GetAsync(ApiRoutes.Payments.Get.Replace("{paymentId}", createdPayment.Id.ToString()));
      var paymentsResponse = response.Content.ReadAsAsync<GetPaymentResponse>().Result;

      Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
      Assert.AreEqual(paymentsResponse.CardNumber, GetMaskedCardNumber(paymentRequest.CardNumber));
      Assert.AreEqual(paymentsResponse.ExpiryDate, paymentRequest.ExpiryDate);
      Assert.AreEqual(paymentsResponse.Amount, paymentRequest.Amount);
      Assert.AreEqual(paymentsResponse.Cvv, paymentRequest.Cvv);
      Assert.AreEqual(paymentsResponse.NameOnCard, paymentRequest.NameOnCard);
    }

    [TestMethod]
    public async Task Get_GetPaymentWithPaymentInDatabaseNotAssignedToUser_Returns404()
    {
      await AuthenticateAsync("user1@test.com");

      var paymentRequest = new CreatePaymentRequest
      {
        CardNumber = "1111111111111111",
        ExpiryDate = DateTime.Now.AddYears(1),
        Amount = 100,
        Cvv = "111",
        NameOnCard = "test name"
      };

      var createdPayment = await CreatePaymentAsync(paymentRequest);

      await AuthenticateAsync("user2@test.com");

      var response = await _testClient.GetAsync(ApiRoutes.Payments.Get.Replace("{paymentId}", createdPayment.Id.ToString()));

      Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Create_CreateAPayment_ReturnsAPaymentId()
    {
      await AuthenticateAsync();

      var paymentRequest = new CreatePaymentRequest
      {
        CardNumber = "1111111111111111",
        ExpiryDate = DateTime.Now.AddYears(1),
        Amount = 100,
        Cvv = "111",
        NameOnCard = "test name"
      };

      var response = await _testClient.PostAsJsonAsync(ApiRoutes.Payments.Create, paymentRequest);
      var createdPayment = await response.Content.ReadAsAsync<CreatePaymentResponse>();

      Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
      Assert.IsTrue(createdPayment.Id.HasValue);
    }


    private string GetMaskedCardNumber(string cardNumber)
    {
      if (string.IsNullOrEmpty(cardNumber))
        return cardNumber;

      List<char> charList = cardNumber.ToList();

      string maskedCardNumber = new string(charList
                                .Select((x, i) => i < charList.Count - 4
                                ? '*' : x).ToArray());
      return maskedCardNumber;
    }

    private async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
    {
      var response = await _testClient.PostAsJsonAsync(ApiRoutes.Payments.Create, request);
      return await response.Content.ReadAsAsync<CreatePaymentResponse>();
    }

  }
}
