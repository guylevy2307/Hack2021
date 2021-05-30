using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Splitit.SDK.Client.Api;
using Splitit.SDK.Client.Client;
using Splitit.SDK.Client.Model;

namespace Hack2021.Controllers
{
    public class SDK_splitIt
    {
        public static void TestApi(string apiKey,string userName,string password,string fullName,string email,string CardNumber, string cvv,string CardExpMonth,string CardExpYear)
        {
            Configuration.Sandbox.AddApiKey("_YOUR_API_KEY_");

            var loginApi = new LoginApi(Configuration.Sandbox);
            var request = new LoginRequest(userName: "_YOUR_USERNAME_", password: "_YOUR_PASSWORD_");

            // Each method also has Async alternative, example: await loginApi.LoginPostAsync(...)
            var loginResult = loginApi.LoginPost(request);

            var installmentPlanApi = new InstallmentPlanApi(Configuration.Sandbox, sessionId: loginResult.SessionId);
            // installmentPlanApi.Culture = "es-ES"; -> optionally set culture for each subsequent request.
            var initResponse = installmentPlanApi.InstallmentPlanInitiate(new InitiateInstallmentPlanRequest()
            {
                PlanData = new PlanData(amount: new MoneyWithCurrencyCode(1000, "USD"), numberOfInstallments: 3),
                BillingAddress = new AddressData()
                {
                    AddressLine = "260 Madison Avenue.",
                    AddressLine2 = "Appartment 1",
                    City = "New York",
                    State = "NY",
                    Country = "USA",
                    Zip = "10016"
                },
                ConsumerData = new ConsumerData(isLocked: true, isDataRestricted: false)
                {
                    FullName = "John Smith",           //*
                    Email = "JohnS@splitit.com",       //*
                    PhoneNumber = "1-844-775-4848",
                    CultureName = "en-us"
                }
            });

            Console.WriteLine("Init call success: " + initResponse.ResponseHeader.Succeeded);

            var createResponse = installmentPlanApi.InstallmentPlanCreate(new CreateInstallmentPlanRequest()
            {
                CreditCardDetails = new CardData()
                {
                    CardNumber = "411111111111111",     //*
                    CardCvv = "123",                    //*
                    CardHolderFullName = "John Smith",   //*
                    CardExpMonth = "12",                 //*
                    CardExpYear = "2022"                 
                },
                InstallmentPlanNumber = initResponse.InstallmentPlan.InstallmentPlanNumber,
                PlanApprovalEvidence = new PlanApprovalEvidence(areTermsAndConditionsApproved: true)
            });

            Console.WriteLine("Create call success: " + createResponse.ResponseHeader.Succeeded);
        }
    }
}
