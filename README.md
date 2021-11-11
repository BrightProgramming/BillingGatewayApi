# Billing Gateway Api

## How to run the solution:

-	Open the solution in Visual Studio 2019.
-	It is made up of 6 projects
    -	Banking.Simulator.Api
    -	Payment.Gateway.Api
    -	Payment.Gateway.Api.TestHarness
    - Banking.Simulator.Api.UnitTests
    - Payment.Gateway.Api.UnitTests
    - Payment.Gateway.Api.IntegrationTests
-	Press F5 to run the solution – it has 3 start up projects:
    - Banking.Simulator.Api
    - Payment.Gateway.Api
    - Payment.Gateway.Api.TestHarness
-	Banking.Simulator.Api should launch a browser window pointing at https://localhost:5002/swagger/index.html This is a swagger endpoint for the Banking Simulator Api. It uses Basic Authentication with a username of bank and a password of bank. The default example should work and return success
    - Press the Authorise button and use a username of bank and a password of bank to authenticate.
    - Expand POST (/api/Payment)
    - Click “Try it out”
    - Press Execute for the default message.
    - You should get a response code of 200 with success of true indicating that the card processing was successful.
    ![image](https://user-images.githubusercontent.com/94113348/141292748-77b249ed-0e15-48b8-b405-904df202802a.png)
    - Repeat the process for an odd card number and you should get a response code of 200 with success of false indicating that the card processing failed.
    - Repeat the process for an expiryMonth of 13 and you should get a response code of 400 indicating that it was a bad request and an error message indicating that the expiryMonth is invalid.
    - Logout and attempt to perform an operation. You should receive a Response code of 401 indicating that the user is not authorized.
- Payment.Gateway.Api should launch a browser window pointing at https://localhost:5001/swagger/index.html This is a swagger endpoint for the Payment Gateway Api. It uses Basic Authentication with a username of user and a password of password. The default examples should work and return success. Note – the Banking Simulator Api must be running on order for this to work.
    - Press the Authorise button and use a username of user and a password of password to authenticate.
    - Expand POST (/api/Payment)
    - Click “Try it out”
    - Press Execute for the default message.
    - You should get a response code of 200 with a payment status of Completed indicating that the card processing was successful.
![image](https://user-images.githubusercontent.com/94113348/141292957-17dec337-4cf7-4c21-8d7a-4cb76f0e66c8.png)
    
    - Repeat the process for an odd card number and you should get a response code of 200 with a Payment status of Rejected indicating that the card processing failed.
    - Repeat the process for an expiryMonth of 13 and you should get a response code of 400 indicating that it was a bad request and an error message indicating that the expiryMonth is invalid.
    - Logout and attempt to perform an operation. You should receive a Response code of 401 indicating that the user is not authorized.
-	Payment.Gateway.Api.TestHarness should launch a console window that allows you to perform an end to end test. Note – it requires the 2 api projects to be running.
    - Initial window should look like:
 ![image](https://user-images.githubusercontent.com/94113348/141293138-b652dbba-918b-49f7-a02b-5ab7a37d2fa5.png)

    - Choosing option 1 allows you to raise a payment request. Press enter at each prompt to use the default value.
 ![image](https://user-images.githubusercontent.com/94113348/141293156-40520d84-222d-4ebb-bd5d-ece53306798d.png)

    - Submit the request to Payment.Gateway.Api (and indirectly to the Banking.Simulator.Api)
    - You should get a response that indicates that the operation was successful.
![image](https://user-images.githubusercontent.com/94113348/141293173-cbc390e7-1de5-437f-a681-9d417cad4b36.png)
    
    - Change the card number to an odd number and you should get a response that indicates that the operation was not successful
![image](https://user-images.githubusercontent.com/94113348/141293207-bda56795-0e28-4e21-9050-1d6622f28541.png)
    
    - Choose option 2 from the main menu to get the details for a submitted payment.
    - Press enter to use the defaults which should work.
![image](https://user-images.githubusercontent.com/94113348/141293239-f449d86e-5648-44de-8d16-7c6217dc0282.png)
    
 
## Assumptions

-	Banking.Simulator.Api should be as simple as possible.
-	Payments.Gateway.Api should show as much best practice as possible.
-	BasicAuthentication is ok as a means of authenticating. It isn’t appropriate to a real implementation but is reasonably for the purposes of this exercise.
-	Secrets would normally come from some form of secret store, but config is reasonable for the purpose of this exercise.

 
## Areas for improvement

-	Jwt authentication
-	Host in some cloud provider. This would give the ability to scale up or down pods depending upon utilization.
-	Use a real database – ideally hosted in the cloud. Current implementation uses mongo2go which is an in memory implementation of MongoDb.
-	Additional time of request validation
-	Additional time on exception handling
-	Additional time on logging. 
-	Potentially use Polly or equivalent for database/interface calls.
-	Move to an async queueing implementation. This would give the ability to replay messages in the event of failure. It would also level out peaks in workloads.

## Possible cloud technologies

Any of the current available cloud providers could be used to host Payments.Gateway.Api.
Payments.Gateway.Api could be hosted in:
-	Gcp – Kubernetes.
-	AWS – EC2
-	Azure – App Service
 
## Technologies Used

-	Mongo2Go as an in memory implementation of MongoDb
-	Basic Authentication
-	Automapper to map between messaging and database models.
-	FluentValidation – to validate inputs.
-	Dependency Injection – for all components.
-	FluentAssertions – for unit tests
-	Middleware  - for global exception handling
-	
 
## Appendix

Banking.Simulator.Api

- Username – bank
- Password – bank
- Endpoints

Endpoint: https://localhost:5002/api/Payment (POST)
Sample message:
{
  "transactionId": "12345678-9861-4C71-9B9F-201EB65E49D0",
  "merchantId": "2CB14EAD-9861-4C71-9B9F-201EB65E49D0",
  "cardNumber": 1234123412341234,
  "expiryMonth": 12,
  "expiryYear": 2023,
  "amount": 10.15,
  "cvv": 123,
  "transactionDate": "2021-10-29T12:05:08.3587598+00:00"
}

 
Payment Gateway Api

- Username – user
- Password – password
- Endpoints:

Endpoint: https://localhost:5001/api/Payment (POST)
Sample message :
{
  "paymentId": "12345678-9861-4C71-9B9F-201EB65E49D0",
  "merchantId": "2CB14EAD-9861-4C71-9B9F-201EB65E49D0",
  "cardNumber": 1234123412341234,
  "expiryMonth": 12,
  "expiryYear": 2023,
  "amount": 10.15,
  "cvv": 123,
  "transactionDate": "2021-10-29T12:05:08.3587598+00:00"
}

Endpoint : https://localhost:5001/api/Payment?paymentId=12345678-9861-4C71-9B9F-201EB65E49D0&merchantId=2CB14EAD-9861-4C71-9B9F-201EB65E49D0 (GET)

