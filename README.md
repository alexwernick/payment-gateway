# payment-gateway

## Overview

A payment gateway has been built to allow a merchant to process a payment. The payment gateway communicates with the acquiring bank and then returns a response to the merchant 

## API Overview

The REST API offers a merchant functionality to be able to process a payment as well as allowing the merchant to see all previously made payments or view a single payment by Id

Swagger has been enabled for this project, so a full specification of the API can be seen by running the solution and viewing https://localhost:44348/swagger/index.html 

The API requires authorisation which must be achieved following the steps below:

* Register a user at /api/v1/identity/register
* The response to this post request will include a token, if successful
* Click the 'Authorize' button at the top right of the page to add the token to future requests. In the value field enter: Bearer {token}
* Once this is done you will be able to use all three of the payment methods
* If you need a new token use /api/v1/identity/login

Please note that the secret being used is set to a test value and will need to be switched for a 32 bit length random string before moving in to production

## Setting up the database

The solution requires a database which is used to keep track of merchant payments as well as user information. Entity framework has been used and therefore the database can be created following the steps below:

* In appsettings.json replace the DefaultConnection string with your connection string
* In the Package manager console run the following two commands
  * PM> Add-Migration Initial -OutputDir "Data/Migrations"
  * PM> Update-database

## The Acquiring Bank

* The Acquiring banks behaviour is currently being mocked but will easily be replaceable once moved into production
* When moving to production the BankPaymentservice will need to be fully implemented and then injected using the ClientInstaller by replacing the BankPaymentSimulationService with the BankPaymentservice
* The BankPaymentSimulationService currently is set to accept 50% of all payments processed though it

## Testing

Unit and integration tests can be seen in the solution. They are self-explanatory so no more detail is needed here

Manually testing the API can be done using the swagger document at https://localhost:44348/swagger/index.html 

## Logging

Additional logging has been put in place when creating a payment. The reason being is that this area of the code is very critical. There is the possibility that a payment is made through the acquiring bank but then fails to be entered into the database. Critical logging has been put in place in the event of this scenario

## Future work

* Implementation of real BankPaymentService
* Refresh token functionality
* Retrying of failed payments
* Getting list of payments by a set of filter parameters
