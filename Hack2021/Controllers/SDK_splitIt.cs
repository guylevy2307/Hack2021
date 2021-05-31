using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hack.Model;
using Splitit.SDK.Client.Api;
using Splitit.SDK.Client.Client;
using Splitit.SDK.Client.Model;

namespace Hack2021.Controllers
{
    public class SDK_splitIt
    {
        public static InstallmentPlanApi PlanApi { get; set; }

        static void Login()
        {
            Configuration.Sandbox.AddApiKey("hY9yHNwYd7c4H6jIEwrcyxOwVHtWW02j41MsCqefmMSd7gvkRO");
            var loginApi = new LoginApi(Configuration.Sandbox);
            var request = new LoginRequest(userName: "APIUser000032868", password: "hY9yHNwYd7c4H6jIEwrcyxOwVHtWW02j41MsCqefmMSd7gvkRO");
            var loginResult = loginApi.LoginPost(request);
            PlanApi = new InstallmentPlanApi(Configuration.Sandbox, sessionId: loginResult.SessionId);
        }
    
        public static void TestApi(string fullName,string email,string CardNumber, string cvv,string CardExpMonth,string CardExpYear)
        {
            Login();
         
            // installmentPlanApi.Culture = "es-ES"; -> optionally set culture for each subsequent request.
            var initResponse = PlanApi.InstallmentPlanInitiate(new InitiateInstallmentPlanRequest()
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

            var createResponse = PlanApi.InstallmentPlanCreate(new CreateInstallmentPlanRequest()
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

        public static void CreateSplit(Transaction t, string installmentPlanNumber)
        {
            Login();

            var createResponse = PlanApi.InstallmentPlanCreate(new CreateInstallmentPlanRequest
            {
                InstallmentPlanNumber = installmentPlanNumber,
                PlanData = new PlanData
                {
                    Amount = new MoneyWithCurrencyCode(Convert.ToDecimal(t.Amount), "USD"),
                    NumberOfInstallments = Int32.Parse(installmentPlanNumber),
                    RefOrderNumber =t.TransactionID.ToString(),
                    AutoCapture = true,
                    PurchaseMethod = PurchaseMethod.ECommerce,
                    FirstChargeDate = DateTime.Now,
                    FirstInstallmentAmount = new MoneyWithCurrencyCode(Convert.ToDecimal(t.Amount)/ Int32.Parse(installmentPlanNumber), "USD"),
                    ExtendedParams = new Dictionary<string, string>
                    {
                        ["AnyParamaterKey1"] = "AnyParameterVal1",
                        ["AnyParameterKey2"] = "AnyParameterVal2",
                    },

                },
                RedirectUrls = new RedirectUrls
                {
                    Canceled = "http://localhost/Canceled",
                    Failed = "http://localhost/Failed",
                    Succeeded = "http://localhost/Succeeded"
                },
                BillingAddress = new AddressData
                {
                    AddressLine = "260 Madison Avenue.",
                    City = "New York",
                    State = "NY",
                    Country = "USA",
                    Zip = "10016"
                },
                ConsumerData = new ConsumerData
                {
                    FullName = t.CreditCardInfo.FullName,
                    Email = t.CreditCardInfo.email,
                    PhoneNumber = "1-415-775-4848",
                    CultureName = "en-us"
                },
                PlanApprovalEvidence = new PlanApprovalEvidence(areTermsAndConditionsApproved: true),
                CreditCardDetails = new CardData
                {
                    CardNumber = t.CreditCardInfo.Number,
                    CardCvv = t.CreditCardInfo.CVV,
                    CardHolderFullName = t.CreditCardInfo.FullName,
                    CardExpMonth = t.CreditCardInfo.ExpirationDate.Month.ToString(),
                    CardExpYear = t.CreditCardInfo.ExpirationDate.Year.ToString(),
                },
            });
        }
    
}
}
